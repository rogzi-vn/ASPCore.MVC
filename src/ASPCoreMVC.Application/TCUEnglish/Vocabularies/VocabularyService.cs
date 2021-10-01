using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using ASPCoreMVC.TCUEnglish.VocabularyTopics;
using ASPCoreMVC.TCUEnglish.WordClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            if (input.IsMustConfirm)
            {
                query = query.Where(x => x.IsConfirmed);
            }

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

        public async Task<ResponseWrapper<PagedResultDto<VocabularySearchResultDTO>>> GetVocabSearchRes(
            GetSearchVocabularyDTO input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => input.Filter.IsNullOrEmpty() || x.Word.Contains(input.Filter));
            if (input.IsMustConfirm)
            {
                query = query.Where(x => x.IsConfirmed);
            }

            //query = query.OrderByDescending(x => x.Word.ToLowerInvariant().StartsWith(input.Filter));
            if (input.Sorting != null)
                query = query.OrderBy(input.Sorting);

            var resQuery = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .Join(_WordClassRepository,
                    v => v.WordClassId,
                    wc => wc.Id,
                    (v, wc) => new {v, wc})
                .Join(_VocabularyTopicRepository,
                    x => x.v.VocabularyTopicId,
                    vc => vc.Id,
                    (x, vc) => new {v = x.v, wc = x.wc, vc})
                .Select(x => new VocabularySearchResultDTO
                {
                    TopicName = x.vc.Name,
                    WordClassName = x.wc.Name,
                    Word = x.v.Word,
                    Mean = x.v.Mean,
                    Pronounce = x.v.Pronounce,
                    PronounceAudio = x.v.PronounceAudio,
                    Explain = x.v.Explain
                });

            var paged = new PagedResultDto<VocabularySearchResultDTO>(query.Count(), resQuery.ToList());
            return new ResponseWrapper<PagedResultDto<VocabularySearchResultDTO>>().SuccessReponseWrapper(paged,
                "Successful");
        }

        public async Task<ResponseWrapper<List<VocabularySimpifyDTO>>> GetRandomVocabularies(int maxCount)
        {
            var query = await Repository.GetQueryableAsync();
            Random rand = new Random();
            int toSkip = rand.Next(0, query.Count());

            var resQuery = query
                .Where(x => x.IsConfirmed)
                .Skip(toSkip)
                .Take(maxCount)
                .Join(_WordClassRepository,
                    v => v.WordClassId,
                    wc => wc.Id,
                    (v, wc) => new {v, wc})
                .Join(_VocabularyTopicRepository,
                    x => x.v.VocabularyTopicId,
                    vc => vc.Id,
                    (x, vc) => new {v = x.v, wc = x.wc, vc})
                .Select(x => new VocabularySimpifyDTO
                {
                    Id = x.v.Id,
                    Word = x.v.Word,
                    Mean = x.v.Mean,
                    PronounceAudio = x.v.PronounceAudio,
                });

            return new ResponseWrapper<List<VocabularySimpifyDTO>>()
                .SuccessReponseWrapper(resQuery.ToList(),
                    "Successful");
        }

        public async Task<PagedResultDto<VocabularyBaseDTO>> GetContributedListAsync(GetVocabularyDTO input)
        {
            var query = await CreateFilteredQueryAsync(input);
            // Get by creator
            query = query.Where(x => x.CreatorId == CurrentUser.Id.Value);
            query = ApplySorting(query, input);

            var totalCount = query.Count();
            query = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);
            var res = ObjectMapper.Map<List<Vocabulary>, List<VocabularyBaseDTO>>(query.ToList());

            return new PagedResultDto<VocabularyBaseDTO>(totalCount, res);
        }

        public async Task<List<QuickVocabularyTestDTO>> GenerateQuickVocabularyTests(int size)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.IgnoreAutoIncludes();
            var qts = new List<QuickVocabularyTestDTO>();
            // Lấy cho đủ số câu hỏi
            var rand = new Random();
            var count = query.Count();
            for (var i = 0; i < size; i++)
            {
                var tempQuery = qts
                    .Aggregate(query, (current, record) =>
                        current.Where(x => x.Id != record.Id));

                // Không lấy những record đã tồn tại trong danh sách kết quả

                var skip = rand.Next(0, count);

                var tempRes = tempQuery
                    .Skip(skip)
                    .Take(1)
                    .Select(x => new QuickVocabularyTestDTO
                    {
                        Id = x.Id,
                        Vocabulary = x.Word,
                        Mean = x.Mean,
                        Answers = new List<string> {x.Mean}
                    })
                    .First();

                if (qts.Any(x => x.Id == tempRes.Id))
                {
                    i--;
                    continue;
                }

                var tempVocabulary = new List<Vocabulary>();

                var tempAnsQuery = query.Where(x => x.Id != tempRes.Id);
                // Không lấy những record đã tồn tại trong danh sách kết quả
                tempAnsQuery = tempVocabulary.Aggregate(tempAnsQuery,
                    (current, record1) => current.Where(x => x.Id != record1.Id));
                var countAns = tempAnsQuery.Count();
                for (var j = 0; j < 3; j++)
                {
                    var skipAns = rand.Next(0, countAns);

                    var tempAns = tempAnsQuery
                        .Skip(skipAns)
                        .Take(1)
                        .First();
                    if (tempVocabulary.Any(x => x.Id == tempAns.Id))
                    {
                        j--;
                        continue;
                    }

                    tempVocabulary.Add(tempAns);
                }

                tempRes.Answers.AddRange(tempVocabulary.Select(x => x.Mean));
                tempRes.Answers = Shuffle(tempRes.Answers);
                qts.Add(tempRes);
            }

            return qts;
        }

        private static List<T> Shuffle<T>(List<T> list)
        {
            var rng = new Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }

        public async Task<dynamic> QuickFix()
        {
            //var res = await Repository.ToListAsync();
            //res = res.Where(x => x.Mean == null || x.Mean.Length <= 0).ToList();
            //for (int i = 0; i < res.Count; i++)
            //{
            //    var regex = new Regex(@"<p>(.*?)<\/p>");
            //    var match = regex.Match(res[i].Explain);
            //    if (match.Success)
            //    {
            //        var mean = match.Groups[1].Value;
            //        var newExplain = res[i].Explain.Replace($"<p>{mean}</p>", "").Trim();
            //        res[i].Mean = mean;
            //        res[i].Explain = newExplain;
            //    }
            //}
            //await Repository.UpdateManyAsync(res);
            //return res;
            throw new Exception("Nố nồ nồ");
        }
    }
}