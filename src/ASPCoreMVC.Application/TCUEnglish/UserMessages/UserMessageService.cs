using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using ASPCoreMVC.TCUEnglish.MesGroups;
using ASPCoreMVC.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.TCUEnglish.UserMessages
{
    public class UserMessageService : CrudAppService<
        UserMessage,
        UserMessageDTO,
        Guid,
        GetUserMessageDTO>, IUserMessageService
    {
        private readonly IRepository<AppUser, Guid> AppUserRepository;
        private readonly IRepository<MessGroup, Guid> MessGroupMessGroup;
        public UserMessageService(IRepository<UserMessage, Guid> repo,
            IRepository<AppUser, Guid> AppUserRepository,
            IRepository<MessGroup, Guid> MessGroupMessGroup) : base(repo)
        {
            this.AppUserRepository = AppUserRepository;
            this.MessGroupMessGroup = MessGroupMessGroup;
        }

        protected override IQueryable<UserMessage> ApplyDefaultSorting(IQueryable<UserMessage> query)
        {
            query = query
                .OrderByDescending(x => x.CreationTime);
            return query;
        }

        public override async Task<PagedResultDto<UserMessageDTO>> GetListAsync(GetUserMessageDTO input)
        {
            var res = await base.GetListAsync(input);
            for (int i = 0; i < res.Items.Count; i++)
            {
                if (res.Items[i].CreatorId != null && res.Items[i].CreatorId != Guid.Empty)
                {
                    var creator = await AppUserRepository.GetAsync(res.Items[i].CreatorId.Value);
                    res.Items[i].DisplayName = creator.DisplayName;
                    res.Items[i].Photo = creator.Picture;
                }
            }
            return res;
        }

        public async Task<List<UserMessageDTO>> GetPreviousMessages(Guid messGroupId, int maxCount, Guid? oldestMsgId)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => x.MessGroupId == messGroupId);

            var unreadMsgQuery = query.Where(x => x.IsReaded == false || x.IsReceived == false);

            query = query.OrderByDescending(x => x.CreationTime);
            if (oldestMsgId == null || oldestMsgId == Guid.Empty)
            {
                query = query.Take(maxCount);
            }
            else
            {
                var currentUserMessage = await Repository.GetAsync(oldestMsgId.Value);
                query = query.Where(x => x.CreationTime < currentUserMessage.CreationTime)
                    .Take(maxCount);
            }
            var currentMsgList = query.ToList();

            // Cập nhật trạng thái là đã nhận, đã xem
            var unreadMsg = unreadMsgQuery.ToList();
            for (int i = 0; i < unreadMsg.Count; i++)
            {
                unreadMsg[i].IsReceived = true;
                unreadMsg[i].IsReaded = true;
                await Repository.UpdateAsync(unreadMsg[i], true);
            }
            return ObjectMapper.Map<List<UserMessage>, List<UserMessageDTO>>(currentMsgList);
        }

        public async Task<int> GetCountUnreadMessage()
        {
            var query = await MessGroupMessGroup.GetQueryableAsync();
            // Lấy danh sách các cuộc hội thoại mà người dùng đang tham gia
            query = query
                 .Where(x => x.Starter == CurrentUser.Id.Value ||
                 x.Members.Contains(CurrentUser.Id.Value.ToString()));
            return query.Join(
                Repository,
                g => g.Id,
                m => m.MessGroupId,
                (g, m) => new { g, m })
                .Where(x => x.m.IsReaded == false && x.m.IsReceived == true)
                .Count();

        }
    }
}
