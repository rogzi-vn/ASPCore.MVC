using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.TCUEnglish.GrammarCategories
{
    public class GrammarCategoryService : WrapperCrudAppService<
        GrammarCategory,
        GrammarCategoryDTO,
        Guid,
        GetGrammarCategoryDTO>, IGrammarCategoryService
    {

        public GrammarCategoryService(IRepository<GrammarCategory, Guid> repo) : base(repo)
        {

        }


        public async Task<ResponseWrapper<PagedResultDto<GrammarCategoryBaseDTO>>> GetBase()
        {
            var list = ObjectMapper.Map<List<GrammarCategory>, List<GrammarCategoryBaseDTO>>(
                                      await Repository.ToListAsync());
            var res = new PagedResultDto<GrammarCategoryBaseDTO>(await Repository.CountAsync(), list);
            return new ResponseWrapper<
                PagedResultDto<GrammarCategoryBaseDTO>>(res, "Successful");
        }

        public async Task<ResponseWrapper<PagedResultDto<GrammarCategoryBaseDTO>>> GetBaseListAsync(GetGrammarCategoryDTO inp)
        {
            var query = await Repository.GetQueryableAsync();
            if (!inp.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(inp.Sorting);
            }
            query = query.Where(x => x.Name.Contains(inp.Filter));

            var resQuery = query.Skip(inp.SkipCount)
                .Take(inp.MaxResultCount);

            var list = ObjectMapper.Map<List<GrammarCategory>, List<GrammarCategoryBaseDTO>>(
                                      resQuery.ToList());
            var res = new PagedResultDto<GrammarCategoryBaseDTO>(query.Count(), list);
            return new ResponseWrapper<
                PagedResultDto<GrammarCategoryBaseDTO>>(res, "Successful");
        }
    }
}
