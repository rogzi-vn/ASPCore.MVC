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
            // Lấy danh sách cuộc hội thoại mà người dùng hiện tại có tham gia
            query = query
                .Where(x => x.Starter == CurrentUser.Id.Value ||
                x.Members.Contains(CurrentUser.Id.Value.ToString()));

            // Nếu bộ lọc có giá trị nhập vào
            if (!input.Filter.IsNullOrEmpty())
            {
                var userQuery = await AppUserRepository.GetQueryableAsync();
                // Lấy danh sách người dùng khớp với từ lọc ngoại trừ user hiện tại
                var allAvailiableUser = userQuery.Where(x => x.Id != CurrentUser.Id &&
                    x.DisplayName.Contains(input.Filter)).ToList();

                IEnumerable<MessGroup> res = new List<MessGroup>();
                foreach (var u in allAvailiableUser)
                {
                    // Tìm room giữa người dùng hiện tại và user này
                    var temp = query
                        .Where(x => x.Starter == u.Id ||
                        x.Members.Contains(u.Id.ToString())).ToList();
                    // Nếu chưa có, tiến hành tạo room
                    if (temp.Count <= 0)
                    {
                        var _mg = new MessGroupDTO
                        {
                            Id = Guid.NewGuid(),
                            Starter = CurrentUser.Id.Value,
                            GroupName = u.DisplayName,
                            Members = u.Id.ToString(),
                            LatestMessage = "",
                            LatestMessageTime = DateTime.Now,
                            UnReadCount = 0,
                            Photo = u.Picture
                        };
                        // Tạo mới cuộc hội thoại
                        var z = await Repository.InsertAsync(MapToEntity(_mg), true);
                        temp.Add(z);
                    }
                    // Thêm từng phần tử vào ds kết quả
                    foreach (var t in temp)
                    {
                        if (!res.Any(x => x.Id == t.Id))
                        {
                            res = res.Append(t);
                        }
                    }
                }
                return res.AsQueryable();
            }

            return query;
        }

        public override async Task<PagedResultDto<MessGroupDTO>> GetListAsync(GetMessGroupDTO input)
        {
            var query = await CreateFilteredQueryAsync(input);

            var userMessageQuery = await UserMessageRepository.GetQueryableAsync();
            userMessageQuery = userMessageQuery.OrderBy(x => x.CreationTime);

            query = ApplyDefaultSorting(query);
            var allMesGroup = ObjectMapper.Map<List<MessGroup>, List<MessGroupDTO>>(query.ToList());

            for (int i = 0; i < allMesGroup.Count; i++)
            {
                var unreadQuery = userMessageQuery
                    .Where(x => x.MessGroupId == allMesGroup[i].Id)
                    .Where(x => x.CreatorId != CurrentUser.Id.Value)
                    .Where(x => x.IsReaded == false)
                    .OrderBy(x => x.CreationTime);

                allMesGroup[i].LatestMessage = userMessageQuery.LastOrDefault()?.Message ?? "";
                allMesGroup[i].UnReadCount = unreadQuery.Count();
                allMesGroup[i].LatestMessageTime = (userMessageQuery.LastOrDefault()?.CreationTime ?? DateTime.Now);
            }

            if (input.Filter.IsNullOrEmpty())
            {
                allMesGroup = allMesGroup.Where(x => x.LatestMessage.Length > 0).ToList();
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
