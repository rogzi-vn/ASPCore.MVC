using ASPCoreMVC.TCUEnglish.GrammarCategories;
using ASPCoreMVC.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.Web.Pages.Manager.GrammarCategories.Partials
{
    [Route("/manager/grammar-categories")]
    public class GrammarCategoryController : Controller
    {
        private readonly IGrammarCategoryService _GrammarCategoryService;
        public GrammarCategoryController(
            IGrammarCategoryService _GrammarCategoryService)
        {
            this._GrammarCategoryService = _GrammarCategoryService;
        }

        [HttpGet]
        [Route("create")]
        public IActionResult GetCreateGrammarGroupAsync()
        {
            return PartialView("~/Pages/Manager/GrammarCategories/Partials/CreateUpdate.cshtml", new GrammarCategoryDTO());
        }

        [HttpGet]
        [Route("update/{id}")]
        public async Task<IActionResult> GetUpdateGrammarGroupAsync(Guid id)
        {
            var res = await _GrammarCategoryService.GetAsync(id);
            if (res.Success)
                return PartialView("~/Pages/Manager/GrammarCategories/Partials/CreateUpdate.cshtml", res.Data);
            else
                return PartialView(AppTheme.ContentNothing);
        }

        [Route("display")]
        [HttpGet]
        public async Task<IActionResult> GetGrammarCategoriessAsync(
             [FromQuery] int? p = 1,
             [FromQuery] string filter = "")
        {
            if (p == null || p <= 0) p = 1;
            ViewBag.p = p.Value;

            var serchInp = new GetGrammarCategoryDTO
            {
                Filter = filter ?? "",
                MaxResultCount = AppTheme.Limit,
                SkipCount = (p.Value - 1) * AppTheme.Limit
            };
            var res = await _GrammarCategoryService.GetBaseListAsync(serchInp);


            if (res.Success)
            {
                PagedResultDto<GrammarCategoryBaseDTO> Containers = res.Data;

                string listRes = string.Format("Showing {0} to {1} of {2} entries",
                    res.Data.TotalCount > 0 ? serchInp.SkipCount + 1 : 0, serchInp.SkipCount + res.Data.Items.Count, res.Data.TotalCount);
                if (!filter.IsNullOrEmpty())
                {
                    listRes += string.Format(" for \"{0}\"", serchInp.Filter);
                }
                ViewBag.ListState = listRes;

                ViewBag.Filter = filter;
                ViewBag.Pagination = PaginateHelper.Generate(
                    "javascript:syncGrammarCategories('{0}', '" + filter + "');",
                    p.Value, Containers.TotalCount, AppTheme.Limit);
                return PartialView("~/Pages/Manager/GrammarCategories/Partials/GrammarCategories.cshtml", Containers);
            }
            else
            {
                return PartialView(AppTheme.ContentNothing);
            }
        }
    }
}
