using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using ASPCoreMVC.TCUEnglish.VocabularyTopics;
using ASPCoreMVC.TCUEnglish.WordClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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
        private readonly IRepository<WordClass, Guid> _WordClassRepository;
        private readonly IRepository<VocabularyTopic, Guid> _VocabularyTopicRepository;
        public VocabularyService(
            IRepository<Vocabulary, Guid> repo,
            IRepository<WordClass, Guid> _WordClassRepository,
            IRepository<VocabularyTopic, Guid> _VocabularyTopicRepository
            ) : base(repo)
        {
            this._WordClassRepository = _WordClassRepository;
            this._VocabularyTopicRepository = _VocabularyTopicRepository;
        }

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
            query = query.Where(x => x.IsConfirmed);
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

        public async Task<ResponseWrapper<PagedResultDto<VocabularySearchResultDTO>>> GetVocabSearchRes(GetSearchVocabularyDTO input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => input.Filter.IsNullOrEmpty() || x.Word.Contains(input.Filter));
            if (input.Sorting != null)
                query = query.OrderBy(input.Sorting);

            var resQuery = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .Join(_WordClassRepository,
                v => v.WordClassId,
                wc => wc.Id,
                (v, wc) => new { v, wc })
                .Join(_VocabularyTopicRepository,
                x => x.v.VocabularyTopicId,
                vc => vc.Id,
                (x, vc) => new { v = x.v, wc = x.wc, vc })
                .Select(x => new VocabularySearchResultDTO
                {
                    TopicName = x.vc.Name,
                    WordClassName = x.wc.Name,
                    Word = x.v.Word,
                    Pronounce = x.v.Pronounce,
                    PronounceAudio = x.v.PronounceAudio,
                    Explain = x.v.Explain
                });

            var paged = new PagedResultDto<VocabularySearchResultDTO>(query.Count(), resQuery.ToList());
            return new ResponseWrapper<PagedResultDto<VocabularySearchResultDTO>>().SuccessReponseWrapper(paged, "Successful");
        }

        public async Task<ResponseWrapper<List<VocabularySimpifyDTO>>> GetRandomVocabularies(int maxCount)
        {
            var query = await Repository.GetQueryableAsync();
            Random rand = new Random();
            int toSkip = rand.Next(0, query.Count());

            var resQuery = query
                .Skip(toSkip)
                .Take(maxCount)
                .Join(_WordClassRepository,
                v => v.WordClassId,
                wc => wc.Id,
                (v, wc) => new { v, wc })
                .Join(_VocabularyTopicRepository,
                x => x.v.VocabularyTopicId,
                vc => vc.Id,
                (x, vc) => new { v = x.v, wc = x.wc, vc })
                .Select(x => new VocabularySimpifyDTO
                {
                    Id = x.v.Id,
                    Word = x.v.Word,
                    PronounceAudio = x.v.PronounceAudio,
                });

            return new ResponseWrapper<List<VocabularySimpifyDTO>>()
                .SuccessReponseWrapper(resQuery.ToList(),
                "Successful");
        }
    }
}
