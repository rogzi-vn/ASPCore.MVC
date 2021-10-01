using ASPCoreMVC.AppUsers;
using ASPCoreMVC.TCUEnglish.MessGroups;
using ASPCoreMVC.TCUEnglish.UserMessages;
using ASPCoreMVC.Users;
using ASPCoreMVC.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.Web.Hubs
{
    [HubRoute("/main-hub")]
    [Authorize]
    public class MainHub : AbpHub
    {
        private readonly IMessGroupService _MessGroupService;
        private readonly IUserMessageService _UserMessageService;

        private readonly IReadOnlyRepository<AppUser, Guid> AppUserRepository;

        private static List<SmallAppUser> UserConnecteds = new List<SmallAppUser>();
        private static List<SmallAppUser> UserInGeneralDiscussRoom = new List<SmallAppUser>();
        private readonly List<Guid> AffectedUser = new List<Guid>();
        private Guid CurrentRoomId = Guid.Empty;

        private readonly IHubContext<NotificationHub> _NotificationHub;

        public MainHub(IReadOnlyRepository<AppUser, Guid> AppUserRepository,
            IMessGroupService _MessGroupService,
            IUserMessageService _UserMessageService,
            IHubContext<NotificationHub> _NotificationHub)
        {
            this.AppUserRepository = AppUserRepository;
            this._MessGroupService = _MessGroupService;
            this._UserMessageService = _UserMessageService;
            this._NotificationHub = _NotificationHub;
        }

        private async Task UpdateOnlineCounter()
        {
            var counter = UserInGeneralDiscussRoom.Count;
            await Clients.All.SendAsync("UpdateGeneralOnlineCounter", counter);
        }

        private async Task GeneralRoomAdd(SmallAppUser currentUser)
        {
            if (UserInGeneralDiscussRoom.All(x => x.Id != currentUser.Id))
            {
                var msg =
                    $"Welcome, \"{currentUser.DisplayName}\" has joined the discussion - {DateTime.Now.ToString("HH:mm")}";
                await Clients
                    .AllExcept(UserConnecteds.Where(x => x.Id == CurrentUser.Id.Value).Select(x => x.ConnectionId)
                        .Append(Context.ConnectionId)).SendAsync("ReceiveWelcome", msg);
                await Clients.Client(Context.ConnectionId).SendAsync("ReceiveWelcome", "You has join the discussion");
            }

            UserInGeneralDiscussRoom.Add(currentUser);
            await UpdateOnlineCounter();
        }

        public override async Task OnConnectedAsync()
        {
            if (CurrentUser.Id != null)
            {
                var currentUser = await AppUserRepository.GetAsync(CurrentUser.Id.Value);

                var mapped = new SmallAppUser
                {
                    Id = currentUser.Id,
                    DisplayName = currentUser.DisplayName,
                    Picture = currentUser.Picture,
                    ConnectionId = Context.ConnectionId
                };

                await GeneralRoomAdd(mapped);

                UserConnecteds.Add(mapped);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var currentUser = UserConnecteds.FirstOrDefault(x => x.Id == CurrentUser.Id.Value);
            UserConnecteds = UserConnecteds.Where(x => x.ConnectionId != Context.ConnectionId).ToList();

            await GeneralRoomRemove(currentUser);
            await base.OnDisconnectedAsync(exception);
        }

        private async Task GeneralRoomRemove(SmallAppUser currentUser)
        {
            var msg = $"\"{currentUser.DisplayName}\" has left the discussion - {DateTime.Now.ToString("HH:mm")}";
            if (UserInGeneralDiscussRoom.Any(x => x.ConnectionId == Context.ConnectionId))
            {
                await Clients
                    .AllExcept(UserConnecteds.Where(x => CurrentUser.Id != null && x.Id == CurrentUser.Id.Value)
                        .Select(x => x.ConnectionId))
                    .SendAsync("ReceiveBye", msg);
                UserInGeneralDiscussRoom =
                    UserInGeneralDiscussRoom.Where(x => x.ConnectionId != Context.ConnectionId).ToList();
                await UpdateOnlineCounter();
            }
        }

        public async Task ChangeRoom(string roomId, string roomName)
        {
            var currentUser =
                UserConnecteds.FirstOrDefault(x => CurrentUser.Id != null && x.Id == CurrentUser.Id.Value);
            if (!roomId.IsNullOrEmpty())
            {
                await GeneralRoomRemove(currentUser);
            }
            else
            {
                await GeneralRoomAdd(currentUser);
            }

            await SendToast("success", string.Format(L["You are changed to room \"{0}\""], roomName));
        }

        private async Task ExtractAffectedUser(string roomId)
        {
            #region Xử lý để lấy danh sách người dùng sẽ nhận tin nhắn

            AffectedUser.Clear();
            var gottedMessGroup = await _MessGroupService.GetAsync(Guid.Parse(roomId));
            AffectedUser.Add(gottedMessGroup.Starter);
            foreach (var id in gottedMessGroup.Members.Split(","))
            {
                AffectedUser.Add(Guid.Parse(id));
            }

            CurrentRoomId = Guid.Parse(roomId);

            #endregion
        }

        public async Task SendMessage(string roomId, string msg)
        {
            if (roomId.IsNullOrEmpty() || roomId.IsNullOrWhiteSpace() || roomId.Length > 5120)
                return;

            if (msg.IsNullOrEmpty() || msg.IsNullOrWhiteSpace() || msg.Length > 5120)
                return;

            await ExtractAffectedUser(roomId);

            var currentUser = UserConnecteds.FirstOrDefault(x => x.Id == CurrentUser.Id.Value);
            var msgDTO = new UserMessageDTO
            {
                Message = msg,
                IsReceived = true,
                IsReaded = false,
                MessGroupId = CurrentRoomId
            };

            // Lưu tin nhắn vào CSDL
            msgDTO = await _UserMessageService.CreateAsync(msgDTO);
            msgDTO.Photo = currentUser.Picture;
            msgDTO.DisplayName = currentUser.DisplayName;

            if (msgDTO.Id != Guid.Empty)
            {
                // Gửi tin nhắn đến tất cả người dùng trong danh sách
                await _NotificationHub.Clients
                    .Users(AffectedUser.Where(x => x != CurrentUser.Id.Value).Select(x => x.ToString()))
                    .SendAsync("NotyHaveNewMsgReciver", msgDTO);
                await Clients.Users(AffectedUser.Select(x => x.ToString())).SendAsync("ReceiveMessage", msgDTO);
            }
        }

        public async Task MessageSeen(UserMessageDTO _msgDTO)
        {
            if (_msgDTO.CreatorId == CurrentUser.Id.Value)
                return;
            _msgDTO.IsReceived = true;
            _msgDTO.IsReaded = true;
            var msgDTO = await _UserMessageService.UpdateAsync(_msgDTO.Id, _msgDTO);
            msgDTO.Photo = _msgDTO.Photo;
            msgDTO.DisplayName = _msgDTO.DisplayName;


            await ExtractAffectedUser(msgDTO.MessGroupId.ToString());

            // Gửi tin nhắn đến tất cả người dùng trong danh sách
            await Clients.Users(AffectedUser.Select(x => x.ToString())).SendAsync("MessageSeenNotify", msgDTO);
        }

        private async Task SendToast(string type, string message)
        {
            await Clients.Caller.SendAsync("OnNotification", type, message);
        }

        public async Task SendGlobalMessage(string msg)
        {
            if (msg.IsNullOrEmpty() || msg.IsNullOrWhiteSpace() || msg.Length > 5120)
                return;

            AffectedUser.Clear();

            var currentUser = UserConnecteds.FirstOrDefault(x => x.Id == CurrentUser.Id.Value);
            var msgDTO = new UserMessageDTO
            {
                MessGroupId = Guid.Empty,
                Message = msg,
                Photo = currentUser.Picture,
                DisplayName = currentUser.DisplayName,
                IsReceived = true,
                CreationTime = DateTime.Now,
                CreatorId = currentUser.Id
            };
            await Clients.All.SendAsync("ReceiveGlobalMessage", msgDTO);
        }
    }
}