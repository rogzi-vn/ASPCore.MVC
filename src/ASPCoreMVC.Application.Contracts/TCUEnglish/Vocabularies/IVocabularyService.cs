using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.Vocabularies
{
    public interface IVocabularyService : IWrapperCrudAppService<
        VocabularyDTO,
        Guid,
        GetVocabularyDTO>
    {
        public Task<ResponseWrapper<PagedResultDto<VocabularyBaseDTO>>> GetBaseListAsync(GetVocabularyDTO input);
        public Task<ResponseWrapper<bool>> PutConfirm(Guid id);
        public Task<ResponseWrapper<bool>> PutNoConfirm(Guid id);
        public Task<ResponseWrapper<List<VocabularyBaseDTO>>> GetAllVocabularies();
    }
}
