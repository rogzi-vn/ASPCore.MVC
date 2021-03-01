using ASPCoreMVC.TCUEnglish.GrammarCategories;
using ASPCoreMVC.TCUEnglish.Grammars;
using ASPCoreMVC.TCUEnglish.Vocabularies;
using ASPCoreMVC.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace ASPCoreMVC.Web.Pages.Dictionary.Vocabularies.Partials
{
    [Route("/dictionary/vocabularies")]
    public class PartialsController : AbpController
    {
        private readonly IVocabularyService _VocabularyService;

        private static string TableView = "~/Pages/Dictionary/Vocabularies/Partials/Table.cshtml";

        public PartialsController(
            IVocabularyService _VocabularyService)
        {
            this._VocabularyService = _VocabularyService;
        }

        [Route("search")]
        [HttpGet]
        public async Task<IActionResult> GetAsync(
            [FromQuery] int? p = 1,
            [FromQuery] string filter = "")
        {
            if (p == null || p <= 0) p = 1;
            ViewBag.p = p.Value;

            if (filter.IsNullOrEmpty())
            {
                return PartialView(TableView, new PagedResultDto<VocabularySearchResultDTO>(0, new List<VocabularySearchResultDTO>()));
            }

            var serchInp = new GetSearchVocabularyDTO
            {
                Filter = filter ?? "",
                MaxResultCount = AppTheme.Limit,
                SkipCount = (p.Value - 1) * AppTheme.Limit,
            };
            var res = await _VocabularyService.GetVocabSearchRes(serchInp);

            if (res.Success)
            {
                PagedResultDto<VocabularySearchResultDTO> Containers = res.Data;

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
                return PartialView(TableView, Containers);
            }
            else
            {
                return PartialView(AppTheme.ContentNothing);
            }
        }

    }
}
