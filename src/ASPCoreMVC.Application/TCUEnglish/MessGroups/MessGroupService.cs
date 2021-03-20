using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using ASPCoreMVC.TCUEnglish.MesGroups;
using ASPCoreMVC.TCUEnglish.UserMessages;
using ASPCoreMVC.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.TCUEnglish.MessGroups
{
    public class MessGroupService : CrudAppService<
        MessGroup,
        MessGroupDTO,
        Guid,
        GetMessGroupDTO>, IMessGroupService
    {
        private readonly IRepository<UserMessage, Guid> UserMessageRepository;
        private readonly IRepository<AppUser, Guid> AppUserRepository;
        public MessGroupService(IRepository<MessGroup, Guid> repo,
            IRepository<UserMessage, Guid> UserMessageRepository,
            IRepository<AppUser, Guid> AppUserRepository) : base(repo)
        {
            this.UserMessageRepository = UserMessageRepository;
            this.AppUserRepository = AppUserRepository;
        }

        protected override IQueryable<MessGroup> ApplyDefaultSorting(IQueryable<MessGroup> query)
        {
            return query
                .OrderByDescending(x => x.CreationTime);
        }

        protected override async Task<IQueryable<MessGroup>> CreateFilteredQueryAsync(GetMessGroupDTO input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query
                .Where(x => x.Starter == CurrentUser.Id)
                .Where(x => x.Members.Contains(CurrentUser.Id.Value.ToString()));
            if (!input.Filter.IsNullOrEmpty())
            {
                var userQuery = await AppUserRepository.GetQueryableAsync();
                var allAvailiableUser = userQuery.Where(x => x.Id != CurrentUser.Id &&
                    x.DisplayName.Contains(input.Filter)).ToList();
                foreach (var u in allAvailiableUser)
                {
                    query = query
                        .Where(x => x.Starter == u.Id)
                        .Where(x => x.Members.Contains(u.Id.ToString()));
                }
            }
            return query;
        }

        public override async Task<PagedResultDto<MessGroupDTO>> GetListAsync(GetMessGroupDTO input)
        {
            var query = await Repository.GetQueryableAsync();

            if (!input.Filter.IsNullOrEmpty())
            {
                query = query.Where(x => x.GroupName.Contains(input.Filter));
            }

            var userMessageQuery = await UserMessageRepository.GetQueryableAsync();

            query = ApplyDefaultSorting(query);
            var allMesGroup = ObjectMapper.Map<List<MessGroup>, List<MessGroupDTO>>(query.ToList());

            #region Nếu có filter nhưng không có cuộc hội thoại nào phù hợp, tiến hành lấy danh sách user
            if (!input.Filter.IsNullOrEmpty() && allMesGroup.Count <= 0)
            {
                var userQuery = await AppUserRepository.GetQueryableAsync();
                userQuery = userQuery
                    .Where(x => x.Id != CurrentUser.Id)
                    .Where(x => x.DisplayName.Contains(input.Filter));
                var matchedUser = userQuery.ToList();
                foreach (var mu in matchedUser)
                {
                    var _mg = new MessGroupDTO
                    {
                        Starter = CurrentUser.Id.Value,
                        GroupName = mu.DisplayName,
                        Members = mu.Id.ToString(),
                        LatestMessage = "",
                        LatestMessageTime = DateTime.Now,
                        UnReadCount = 0,
                        Photo = mu.Picture
                    };
                    allMesGroup.Add(_mg);
                }
            }
            #endregion

            for (int i = 0; i < allMesGroup.Count; i++)
            {
                var unreadQuery = userMessageQuery
                    .Where(x => x.MessGroupId == allMesGroup[i].Id)
                    .Where(x => x.IsReaded == false)
                    .OrderBy(x => x.CreationTime);

                allMesGroup[i].LatestMessage = unreadQuery.LastOrDefault()?.Message ?? "";
                allMesGroup[i].UnReadCount = unreadQuery.Count();
                allMesGroup[i].LatestMessageTime = (unreadQuery.LastOrDefault()?.CreationTime ?? DateTime.Now);
            }

            var res = allMesGroup.Skip(input.SkipCount).Take(input.MaxResultCount);
            var returnResult = new PagedResultDto<MessGroupDTO>(allMesGroup.Count(), res.ToList());
            for (int i = 0; i < returnResult.Items.Count; i++)
            {
                var temp = returnResult.Items[i];
                var userIds = new List<Guid>();
                if (temp.Starter != CurrentUser.Id)
                    userIds.Add(temp.Starter);
                foreach (var item in temp.Members.Split(","))
                {
                    var crrGuid = Guid.Parse(item);
                    if (crrGuid != CurrentUser.Id)
                        userIds.Add(crrGuid);
                }
                if (userIds.Count > 0)
                {
                    var destUser = await AppUserRepository.GetAsync(userIds.FirstOrDefault());
                    returnResult.Items[i].Photo = destUser.Picture;
                    returnResult.Items[i].GroupName = destUser.DisplayName;
                }
            }

            return returnResult;
        }
    }
}
