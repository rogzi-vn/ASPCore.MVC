using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
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
        public UserMessageService(IRepository<UserMessage, Guid> repo,
            IRepository<AppUser, Guid> AppUserRepository) : base(repo)
        {
            this.AppUserRepository = AppUserRepository;
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
            return ObjectMapper.Map<List<UserMessage>, List<UserMessageDTO>>(query.ToList());
        }
    }
}
