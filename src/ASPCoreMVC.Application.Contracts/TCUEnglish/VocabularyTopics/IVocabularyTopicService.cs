using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPCoreMVC.TCUEnglish.VocabularyTopics
{
    public interface IVocabularyTopicService : IWrapperCrudAppService<VocabularyTopicDTO, Guid, GetVocabularyTopicDTO>
    {
        public Task<ResponseWrapper<bool>> PutConfirm(Guid id);
        public Task<ResponseWrapper<bool>> PutNoConfirm(Guid id);
        public Task<ResponseWrapper<List<VocabularyTopicDTO>>> GetAllVocabularyTopics();
    }
}
