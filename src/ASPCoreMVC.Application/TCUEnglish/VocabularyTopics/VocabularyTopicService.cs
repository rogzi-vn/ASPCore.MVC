using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.TCUEnglish.VocabularyTopics
{
    public class VocabularyTopicService : WrapperCrudAppService<
        VocabularyTopic,
        VocabularyTopicDTO,
        Guid,
        GetVocabularyTopicDTO>, IVocabularyTopicService
    {
        public VocabularyTopicService(IRepository<VocabularyTopic, Guid> repo) : base(repo) { }

        public async Task<ResponseWrapper<List<VocabularyTopicDTO>>> GetAllVocabularyTopics()
        {
            return new ResponseWrapper<List<VocabularyTopicDTO>>()
                .SuccessReponseWrapper(ObjectMapper.Map<
                List<VocabularyTopic>, List<VocabularyTopicDTO>>(await Repository.GetListAsync()), "Successful");
        }

        public async Task<ResponseWrapper<bool>> PutConfirm(Guid id)
        {
            var vt = await Repository.GetAsync(id);
            if (vt == null)
                return new ResponseWrapper<bool>().ErrorReponseWrapper(false, "Not found", 404);
            else
            {
                vt.IsConfirmed = true;
                vt.ConfirmerId = CurrentUser.Id;
                vt.ConfirmedTime = DateTime.Now;
                return new ResponseWrapper<bool>().SuccessReponseWrapper(true, "Successful");
            }
        }

        public async Task<ResponseWrapper<bool>> PutNoConfirm(Guid id)
        {
            var vt = await Repository.GetAsync(id);
            if (vt == null)
                return new ResponseWrapper<bool>().ErrorReponseWrapper(false, "Not found", 404);
            else
            {
                vt.IsConfirmed = false;
                vt.ConfirmerId = null;
                vt.ConfirmedTime = null;
                return new ResponseWrapper<bool>().SuccessReponseWrapper(true, "Successful");
            }
        }

        protected override async Task<IQueryable<VocabularyTopic>> CreateFilteredQueryAsync(GetVocabularyTopicDTO input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => x.IsConfirmed);
            return query.Where(x => input.Filter.IsNullOrEmpty() || x.Name.Contains(input.Filter));
        }
    }
}
