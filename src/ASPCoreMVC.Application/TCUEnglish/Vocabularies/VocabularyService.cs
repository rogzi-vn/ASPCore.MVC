using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.TCUEnglish.Vocabularies
{
    public class VocabularyService : WrapperCrudAppService<
        Vocabulary,
        VocabularyDTO,
        Guid,
        GetVocabularyDTO>, IVocabularyService
    {
        public VocabularyService(IRepository<Vocabulary, Guid> repo) : base(repo) { }

        public async Task<ResponseWrapper<PagedResultDto<VocabularyBaseDTO>>> GetBaseListAsync(GetVocabularyDTO input)
        {
            var _res = await GetListAsync(input);
            var paged = new PagedResultDto<VocabularyBaseDTO>(_res.Data.TotalCount,
               ObjectMapper.Map<IReadOnlyList<VocabularyDTO>, IReadOnlyList<VocabularyBaseDTO>>(_res.Data.Items));
            return new ResponseWrapper<PagedResultDto<VocabularyBaseDTO>>().SuccessReponseWrapper(paged, _res.Message);
        }

        protected override async Task<IQueryable<Vocabulary>> CreateFilteredQueryAsync(GetVocabularyDTO input)
        {
            var query = await Repository.GetQueryableAsync();
            if (input.VocabularyTopicId != null && input.VocabularyTopicId != Guid.Empty)
                query = query.Where(x => x.VocabularyTopicId == input.VocabularyTopicId);

            if (input.WordClassId != null && input.WordClassId != Guid.Empty)
                query = query.Where(x => x.WordClassId == input.WordClassId);

            return query.Where(x => input.Filter.IsNullOrEmpty() || x.Word.Contains(input.Filter));
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

        public async Task<ResponseWrapper<List<VocabularyBaseDTO>>> GetAllVocabularies()
        {
            return new ResponseWrapper<List<VocabularyBaseDTO>>()
                .SuccessReponseWrapper(ObjectMapper.Map<
                List<Vocabulary>, List<VocabularyBaseDTO>>(await Repository.GetListAsync()), "Successful");
        }
    }
}
