using ASPCoreMVC.TCUEnglish.GrammarCategories;
using ASPCoreMVC.TCUEnglish.Grammars;
using ASPCoreMVC.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace ASPCoreMVC.Web.Pages.Dictionary.Grammars.Partials.Grammars
{
    [Route("/dictionary/grammars")]
    public class PartialsController : AbpController
    {
        private readonly IGrammarService _GrammarService;
        private readonly IGrammarCategoryService _GrammarCategoryService;

        private static string CreateUpdateView = "~/Pages/Dictionary/Grammars/Partials/Grammars/CreateUpdate.cshtml";
        private static string TableView = "~/Pages/Dictionary/Grammars/Partials/Grammars/Table.cshtml";

        public PartialsController(
            IGrammarService _GrammarService,
            IGrammarCategoryService _GrammarCategoryService)
        {
            this._GrammarService = _GrammarService;
            this._GrammarCategoryService = _GrammarCategoryService;
        }

        [Route("display")]
        [HttpGet]
        public async Task<IActionResult> GetWordClassesAsync(
            [FromQuery] Guid? gcId,
            [FromQuery] int? p = 1,
            [FromQuery] string filter = "")
        {
            if (p == null || p <= 0) p = 1;
            ViewBag.p = p.Value;

            ViewBag.Cate = L["All"];
            if (gcId != null && gcId != Guid.Empty)
            {
                var gc = await _GrammarCategoryService.GetAsync(gcId.Value);
                if (gc.Success && gc.Data != null)
                {
                    ViewBag.Cate = gc.Data.Name;
                }
            }

            var serchInp = new GetGrammarDTO
            {
                Filter = filter ?? "",
                MaxResultCount = AppTheme.Limit,
                SkipCount = (p.Value - 1) * AppTheme.Limit,
                GrammarCategoryId = gcId
            };
            var res = await _GrammarService.GetBaseListAsync(serchInp);

            if (res.Success)
            {
                PagedResultDto<GrammarBaseDTO> Containers = res.Data;

                string listRes = string.Format("Showing {0} to {1} of {2} entries",
                    res.Data.TotalCount > 0 ? serchInp.SkipCount + 1 : 0, serchInp.SkipCount + res.Data.Items.Count, res.Data.TotalCount);
                if (!filter.IsNullOrEmpty())
                {
                    listRes += string.Format(" for \"{0}\"", serchInp.Filter);
                }
                ViewBag.ListState = listRes;

                ViewBag.Filter = filter;
                ViewBag.Pagination = PaginateHelper.Generate(
                    "javascript:syncVt('" + gcId + "','{0}', '" + filter + "');",
                    p.Value, Containers.TotalCount, AppTheme.Limit);
                return PartialView(TableView, Containers);
            }
            else
            {
                return PartialView(AppTheme.ContentNothing);
            }
        }

        [HttpGet]
        [Route("create")]
        public IActionResult GetCreateAsync()
        {
            return PartialView(CreateUpdateView);
        }

        [HttpGet]
        [Route("update/{id}")]
        public async Task<IActionResult> GetUpdateAsync(Guid id)
        {
            var res = await _GrammarService.GetAsync(id);
            if (res.Success)
                return PartialView(CreateUpdateView, res.Data);
            else
                return PartialView(AppTheme.ContentNothing);
        }

        [HttpGet]
        [Route("{id:Guid}/detail")]
        public async Task<IActionResult> Detail(Guid id)
        {
            if (id == Guid.Empty)
                return PartialView(AppTheme.ContentNothing);

            var res = await _GrammarService.GetAsync(id);
            if (res.Success && res.Data != null)
                return PartialView("~/Pages/Dictionary/Grammars/Partials/Grammars/Detail.cshtml", res.Data);
            else
                return PartialView(AppTheme.ContentNothing);
        }
    }
}
