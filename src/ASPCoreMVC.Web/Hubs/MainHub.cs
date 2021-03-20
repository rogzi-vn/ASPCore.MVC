using ASPCoreMVC.AppUsers;
using ASPCoreMVC.TCUEnglish.UserMessages;
using ASPCoreMVC.Users;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.Web.Hubs
{
    [HubRoute("/main-hub")]
    public class MainHub : AbpHub
    {
        private readonly IReadOnlyRepository<AppUser, Guid> AppUserRepository;
        public MainHub(IReadOnlyRepository<AppUser, Guid> AppUserRepository)
        {
            this.AppUserRepository = AppUserRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var currentUser = await AppUserRepository.GetAsync(CurrentUser.Id.Value);
            var msg = string.Format("Welcome, \"{0}\" has joined the discussion - {1}",
                currentUser.DisplayName, DateTime.Now.ToString("HH:mm"));
            await Clients.All.SendAsync("ReceiveWelcome", msg);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            Console.WriteLine(exception.Message);
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            var currentUser = await AppUserRepository.GetAsync(CurrentUser.Id.Value);
            var msg = string.Format("\"{0}\" has left the discussion - {1}",
                currentUser.DisplayName, DateTime.Now.ToString("HH:mm"));
            await Clients.All.SendAsync("ReceiveBye", msg);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendGlobalMessage(string msg)
        {
            if (msg.IsNullOrEmpty() || msg.IsNullOrWhiteSpace() || msg.Length > 5120)
                return;

            var currentUser = await AppUserRepository.GetAsync(CurrentUser.Id.Value);
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

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
