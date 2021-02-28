using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.GrammarCategories
{
    public interface IGrammarCategoryService : IWrapperCrudAppService<
        GrammarCategoryDTO, Guid, GetGrammarCategoryDTO>
    {
        public Task<ResponseWrapper<PagedResultDto<GrammarCategoryBaseDTO>>> GetBase();
        public Task<ResponseWrapper<PagedResultDto<GrammarCategoryBaseDTO>>> GetBaseListAsync(GetGrammarCategoryDTO inp);
    }
}
