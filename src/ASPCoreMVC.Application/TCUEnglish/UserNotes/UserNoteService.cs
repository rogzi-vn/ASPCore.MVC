using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.TCUEnglish.UserNotes
{
    public class UserNoteService : WrapperCrudAppService<
        UserNote,
        UserNoteDTO,
        Guid,
        GetUserNoteDTO>, IUserNoteService
    {
        public UserNoteService(IRepository<UserNote, Guid> repo) : base(repo) { }

        protected override async Task<IQueryable<UserNote>> CreateFilteredQueryAsync(GetUserNoteDTO input)
        {
            var query = await Repository.GetQueryableAsync();
            return query.Where(x => x.CreatorId == CurrentUser.Id);
        }

        public async Task<ResponseWrapper<PagedResultDto<UserNoteBaseDTO>>> GetBaseListAsync(GetUserNoteDTO input)
        {
            var query = await Repository.GetQueryableAsync();
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }
            query = query.Where(x => x.CreatorId == CurrentUser.Id);
            query = query.Where(x => x.Title.Contains(input.Filter));

            var resQuery = query.Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var list = ObjectMapper.Map<List<UserNote>, List<UserNoteBaseDTO>>(
                                      resQuery.ToList());
            var res = new PagedResultDto<UserNoteBaseDTO>(query.Count(), list);
            return new ResponseWrapper<
                PagedResultDto<UserNoteBaseDTO>>(res, "Successful");
        }
    }
}
