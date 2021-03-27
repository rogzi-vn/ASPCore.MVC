using ASPCoreMVC.TCUEnglish.Vocabularies;
using ASPCoreMVC.TCUEnglish.VocabularyTopics;
using ASPCoreMVC.TCUEnglish.WordClasses;
using ASPCoreMVC.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace ASPCoreMVC.Web.Pages.Dictionary.VocabContributes.Partials
{
    [Authorize]
    [Route("/dictionary/vocabularies/contribute")]
    public class VocabContributeController : AbpController
    {
        private readonly IVocabularyService _VocabularyService;
        private readonly IWordClassService _WordClassService;
        private readonly IVocabularyTopicService _VocabularyTopicService;

        public VocabContributeController(
            IVocabularyService _VocabularyService,
            IWordClassService _WordClassService,
            IVocabularyTopicService _VocabularyTopicService)
        {
            this._VocabularyService = _VocabularyService;
            this._WordClassService = _WordClassService;
            this._VocabularyTopicService = _VocabularyTopicService;
        }

        [Route("vocabulary-display")]
        [HttpGet]
        public async Task<IActionResult> GetVocabularyAsync(
            [FromQuery] int? p = 1,
            [FromQuery] string filter = "")
        {
            if (p == null || p <= 0) p = 1;
            ViewBag.p = p.Value;

            var serchInp = new GetVocabularyDTO
            {
                Filter = filter ?? "",
                MaxResultCount = AppTheme.Limit,
                SkipCount = (p.Value - 1) * AppTheme.Limit,
                IsMustConfirm = false
            };
            var Containers = await _VocabularyService.GetContributedListAsync(serchInp);

            string listRes = string.Format("Showing {0} to {1} of {2} entries",
                Containers.TotalCount > 0 ? serchInp.SkipCount + 1 : 0, serchInp.SkipCount + Containers.Items.Count, Containers.TotalCount);
            if (!filter.IsNullOrEmpty())
            {
                listRes += string.Format(" for \"{0}\"", serchInp.Filter);
            }
            ViewBag.ListState = listRes;

            ViewBag.Filter = filter;
            ViewBag.Pagination = PaginateHelper.Generate(
                "javascript:syncVt('{0}', '" + filter + "');",
                p.Value, Containers.TotalCount, AppTheme.Limit);
            return PartialView("~/Pages/Dictionary/VocabContributes/Partials/Vocabularies.cshtml", Containers);

        }

        [Route("topic-display")]
        [HttpGet]
        public async Task<IActionResult> GetTopicAsync(
           [FromQuery] int? p = 1,
           [FromQuery] string filter = "")
        {
            if (p == null || p <= 0) p = 1;
            ViewBag.p = p.Value;

            var serchInp = new GetVocabularyTopicDTO
            {
                Filter = filter ?? "",
                MaxResultCount = AppTheme.Limit,
                SkipCount = (p.Value - 1) * AppTheme.Limit,
                IsMustConfirm = false
            };
            var Containers = await _VocabularyTopicService.GetContributedVocabularyTopicAsync(serchInp);

            string listRes = string.Format("Showing {0} to {1} of {2} entries",
                Containers.TotalCount > 0 ? serchInp.SkipCount + 1 : 0, serchInp.SkipCount + Containers.Items.Count, Containers.TotalCount);
            if (!filter.IsNullOrEmpty())
            {
                listRes += string.Format(" for \"{0}\"", serchInp.Filter);
            }
            ViewBag.ListState = listRes;

            ViewBag.Filter = filter;
            ViewBag.Pagination = PaginateHelper.Generate(
                "javascript:syncVt('{0}', '" + filter + "');",
                p.Value, Containers.TotalCount, AppTheme.Limit);
            return PartialView("~/Pages/Dictionary/VocabContributes/Partials/Topics.cshtml", Containers);

        }

        [HttpGet]
        [Route("create-vocabulary")]
        public async Task<IActionResult> GetCreateAsync()
        {
            ViewBag.VocabularyTopics = (await _VocabularyTopicService.GetAllVocabularyTopics()).Data;
            ViewBag.WordClasses = (await _WordClassService.GetAllWordClasses()).Data;
            return PartialView("~/Pages/Dictionary/VocabContributes/Partials/CreateUpdateVocabulary.cshtml",
                new VocabularyDTO
                {
                });
        }

        [HttpGet]
        [Route("update-vocabulary/{id}")]
        public async Task<IActionResult> GetUpdateAsync(Guid id)
        {
            var res = await _VocabularyService.GetAsync(id);
            if (res.Success)
            {
                ViewBag.VocabularyTopics = (await _VocabularyTopicService.GetAllVocabularyTopics()).Data;
                ViewBag.WordClasses = (await _WordClassService.GetAllWordClasses()).Data;
                return PartialView("~/Pages/Dictionary/VocabContributes/Partials/CreateUpdateVocabulary.cshtml", res.Data);
            }
            else
                return PartialView(AppTheme.ContentNothing);
        }

        [HttpGet]
        [Route("create-topic")]
        public IActionResult CreateTopic()
        {
            return PartialView("~/Pages/Dictionary/VocabContributes/Partials/CreateUpdateTopic.cshtml",
                new VocabularyTopicDTO
                {
                });
        }

        [HttpGet]
        [Route("update-topic/{id}")]
        public async Task<IActionResult> UpdateTopic(Guid id)
        {
            var res = await _VocabularyTopicService.GetAsync(id);
            if (res.Success)
                return PartialView("~/Pages/Dictionary/VocabContributes/Partials/CreateUpdateTopic.cshtml", res.Data);
            else
                return PartialView(AppTheme.ContentNothing);
        }
    }
}
