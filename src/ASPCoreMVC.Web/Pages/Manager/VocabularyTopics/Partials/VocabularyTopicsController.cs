using ASPCoreMVC.Permissions;
using ASPCoreMVC.TCUEnglish.VocabularyTopics;
using ASPCoreMVC.TCUEnglish.WordClasses;
using ASPCoreMVC.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.Web.Pages.Manager.VocabularyTopics.Partials
{
    [Authorize(ASPCoreMVCPermissions.VocabularyTopicManager.Default)]
    [Route("/manager/vocabulary-topics")]
    public class VocabularyTopicsController : Controller
    {
        private readonly IVocabularyTopicService _VocabularyTopicService;

        public VocabularyTopicsController(
            IVocabularyTopicService _VocabularyTopicService)
        {
            this._VocabularyTopicService = _VocabularyTopicService;
        }

        [Route("display")]
        [HttpGet]
        public async Task<IActionResult> GetAsync(
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
            var res = await _VocabularyTopicService.GetListAsync(serchInp);

            if (res.Success)
            {
                PagedResultDto<VocabularyTopicDTO> Containers = res.Data;

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
                return PartialView("~/Pages/Manager/VocabularyTopics/Partials/VocabularyTopics.cshtml", Containers);
            }
            else
            {
                return PartialView(AppTheme.ContentNothing);
            }
        }

        [HttpGet]
        [Route("create")]
        public IActionResult GetCreateWCAsync()
        {
            return PartialView("~/Pages/Manager/VocabularyTopics/Partials/CreateUpdate.cshtml",
                new VocabularyTopicDTO
                {
                });
        }

        [HttpGet]
        [Route("update/{id}")]
        public async Task<IActionResult> GetUpdateWCAsync(Guid id)
        {
            var res = await _VocabularyTopicService.GetAsync(id);
            if (res.Success)
                return PartialView("~/Pages/Manager/VocabularyTopics/Partials/CreateUpdate.cshtml", res.Data);
            else
                return PartialView(AppTheme.ContentNothing);
        }
    }
}
