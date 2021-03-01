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

namespace ASPCoreMVC.TCUEnglish.Grammars
{
    public class GrammarService : WrapperCrudAppService<
        Grammar,
        GrammarDTO,
        Guid,
        GetGrammarDTO>, IGrammarService
    {
        public GrammarService(IRepository<Grammar, Guid> repo) : base(repo)
        {

        }

        public async Task<ResponseWrapper<List<GrammarSimpify>>> GetAllSimpifyAsync()
        {
            var res = await Repository.GetListAsync();
            return new ResponseWrapper<List<GrammarSimpify>>()
                .SuccessReponseWrapper(ObjectMapper.Map<List<Grammar>, List<GrammarSimpify>>(res), "Successful");
        }

        public async Task<ResponseWrapper<PagedResultDto<GrammarBaseDTO>>> GetBase(Guid ggId)
        {
            var list = ObjectMapper.Map<List<Grammar>, List<GrammarBaseDTO>>(
                                      Repository.Where(x => x.GrammarCategoryId == ggId).ToList());
            var res = new PagedResultDto<GrammarBaseDTO>(await Repository.CountAsync(), list);
            return new ResponseWrapper<
                PagedResultDto<GrammarBaseDTO>>(res, "Successful");
        }

        public async Task<ResponseWrapper<PagedResultDto<GrammarBaseDTO>>> GetBaseListAsync(GetGrammarDTO inp)
        {
            var query = await Repository.GetQueryableAsync();
            if (!inp.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(inp.Sorting);
            }
            if (inp.GrammarCategoryId != null && inp.GrammarCategoryId != Guid.Empty)
            {
                query = query.Where(x => x.GrammarCategoryId == inp.GrammarCategoryId);
            }
            query = query.Where(x => x.Name.Contains(inp.Filter));

            var resQuery = query.Skip(inp.SkipCount)
                .Take(inp.MaxResultCount);

            var list = ObjectMapper.Map<List<Grammar>, List<GrammarBaseDTO>>(
                                      resQuery.ToList());
            var res = new PagedResultDto<GrammarBaseDTO>(query.Count(), list);
            return new ResponseWrapper<
                PagedResultDto<GrammarBaseDTO>>(res, "Successful");
        }

        public async Task<ResponseWrapper<long>> GetCount(Guid ggId)
        {
            var q = await Repository.GetQueryableAsync();
            if (ggId != Guid.Empty)
            {
                q = q.Where(x => x.GrammarCategoryId == ggId);
            }
            return new ResponseWrapper<long>().SuccessReponseWrapper(q.LongCount(), "Successful");
        }

        public async Task<ResponseWrapper<long>> GetCountAll()
        {
            var q = await Repository.GetQueryableAsync();
            return new ResponseWrapper<long>().SuccessReponseWrapper(q.LongCount(), "Successful");
        }
    }
}
