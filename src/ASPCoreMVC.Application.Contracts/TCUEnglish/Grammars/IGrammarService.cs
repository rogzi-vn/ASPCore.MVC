using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.Grammars
{
    public interface IGrammarService : IWrapperCrudAppService<
        GrammarDTO, Guid, GetGrammarDTO>
    {
        public Task<ResponseWrapper<PagedResultDto<GrammarBaseDTO>>> GetBase(Guid ggId);
        public Task<ResponseWrapper<long>> GetCount(Guid ggId);
        public Task<ResponseWrapper<long>> GetCountAll();
        public Task<ResponseWrapper<PagedResultDto<GrammarBaseDTO>>> GetBaseListAsync(GetGrammarDTO inp);
    }
}
