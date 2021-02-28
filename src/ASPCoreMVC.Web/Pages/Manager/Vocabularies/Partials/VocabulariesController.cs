using ASPCoreMVC.TCUEnglish.Vocabularies;
using ASPCoreMVC.TCUEnglish.VocabularyTopics;
using ASPCoreMVC.TCUEnglish.WordClasses;
using ASPCoreMVC.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.Web.Pages.Manager.Vocabularies.Partials
{
    [Route("/manager/vocabularies")]
    public class VocabulariesController : Controller
    {
        private readonly IVocabularyService _VocabularyService;
        private readonly IWordClassService _WordClassService;
        private readonly IVocabularyTopicService _VocabularyTopicService;

        public VocabulariesController(
            IVocabularyService _VocabularyService,
            IWordClassService _WordClassService,
            IVocabularyTopicService _VocabularyTopicService)
        {
            this._VocabularyService = _VocabularyService;
            this._WordClassService = _WordClassService;
            this._VocabularyTopicService = _VocabularyTopicService;
        }

        [Route("display")]
        [HttpGet]
        public async Task<IActionResult> GetAsync(
            [FromQuery] Guid? topicId,
            [FromQuery] Guid? wcId,
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
                WordClassId = wcId,
                VocabularyTopicId = topicId
            };
            var res = await _VocabularyService.GetListAsync(serchInp);

            if (res.Success)
            {
                PagedResultDto<VocabularyDTO> Containers = res.Data;

                string listRes = string.Format("Showing {0} to {1} of {2} entries",
                    res.Data.TotalCount > 0 ? serchInp.SkipCount + 1 : 0, serchInp.SkipCount + res.Data.Items.Count, res.Data.TotalCount);
                if (!filter.IsNullOrEmpty())
                {
                    listRes += string.Format(" for \"{0}\"", serchInp.Filter);
                }
                ViewBag.ListState = listRes;

                ViewBag.Filter = filter;
                ViewBag.Pagination = PaginateHelper.Generate(
                    "javascript:syncVt('{0}', '" + filter + "');",
                    p.Value, Containers.TotalCount, AppTheme.Limit);
                return PartialView("~/Pages/Manager/Vocabularies/Partials/Vocabularies.cshtml", Containers);
            }
            else
            {
                return PartialView(AppTheme.ContentNothing);
            }
        }

        [HttpGet]
        [Route("create")]
        public async Task<IActionResult> GetCreateAsync()
        {
            ViewBag.VocabularyTopics = (await _VocabularyTopicService.GetAll()).Data;
            ViewBag.WordClasses = (await _WordClassService.GetAll()).Data;
            return PartialView("~/Pages/Manager/Vocabularies/Partials/CreateUpdate.cshtml",
                new VocabularyDTO
                {
                });
        }

        [HttpGet]
        [Route("update/{id}")]
        public async Task<IActionResult> GetUpdateAsync(Guid id)
        {
            var res = await _VocabularyService.GetAsync(id);
            if (res.Success)
            {
                ViewBag.VocabularyTopics = (await _VocabularyTopicService.GetAll()).Data;
                ViewBag.WordClasses = (await _WordClassService.GetAll()).Data;
                return PartialView("~/Pages/Manager/Vocabularies/Partials/CreateUpdate.cshtml", res.Data);
            }
            else
                return PartialView(AppTheme.ContentNothing);
        }
    }
}
