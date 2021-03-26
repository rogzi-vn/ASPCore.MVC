using ASPCoreMVC.Permissions;
using ASPCoreMVC.TCUEnglish.GrammarCategories;
using ASPCoreMVC.TCUEnglish.Grammars;
using ASPCoreMVC.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace ASPCoreMVC.Web.Pages.Manager.Grammars.Partials
{
    [Authorize(ASPCoreMVCPermissions.GrammarManager.Default)]
    [Route("/manager/grammars")]
    public class GrammarController : AbpController
    {
        private readonly IGrammarCategoryService _GrammarCategoryService;
        private readonly IGrammarService _Grammarervice;
        public GrammarController(
            IGrammarCategoryService _GrammarCategoryService,
            IGrammarService _Grammarervice)
        {
            this._GrammarCategoryService = _GrammarCategoryService;
            this._Grammarervice = _Grammarervice;
        }

        [HttpGet]
        [Route("{gcId}/create")]
        public IActionResult GetCreateGrammarAsync(Guid gcId)
        {
            if (gcId == Guid.Empty)
                return PartialView(AppTheme.ContentNothing);
            return PartialView("~/Pages/Manager/Grammars/Partials/CreateUpdate.cshtml",
                new GrammarDTO
                {
                    GrammarCategoryId = gcId
                });
        }

        [HttpGet]
        [Route("update/{id}")]
        public async Task<IActionResult> GetUpdateGrammarGroupAsync(Guid id)
        {
            var res = await _Grammarervice.GetAsync(id);
            if (res.Success)
                return PartialView("~/Pages/Manager/Grammars/Partials/CreateUpdate.cshtml", res.Data);
            else
                return PartialView(AppTheme.ContentNothing);
        }


        [Route("display")]
        [HttpGet]
        public async Task<IActionResult> GetQuestionsAsync(
           [FromQuery] Guid? gcId,
            [FromQuery] int? p = 1,
            [FromQuery] string filter = "")
        {
            if (p == null || p <= 0) p = 1;
            ViewBag.p = p.Value;

            var serchInp = new GetGrammarDTO
            {
                Filter = filter ?? "",
                MaxResultCount = AppTheme.Limit,
                SkipCount = (p.Value - 1) * AppTheme.Limit,
                GrammarCategoryId = gcId ?? null
            };
            var res = await _Grammarervice.GetBaseListAsync(serchInp);
            if (gcId == null || gcId == Guid.Empty)
            {
                ViewBag.GroupName = L["All"];
            }
            else
            {
                var qg = await _GrammarCategoryService.GetAsync(gcId.Value);
                if (!qg.Success || qg.Data == null)
                    ViewBag.GroupName = L["Unknow"];
                else
                    ViewBag.GroupName = qg.Data.Name;
            }
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
                    "javascript:syncGrammar('" + gcId + "','{0}', '" + filter + "');",
                    p.Value, Containers.TotalCount, AppTheme.Limit);
                return PartialView("~/Pages/Manager/Grammars/Partials/Grammars.cshtml", Containers);
            }
            else
            {
                return PartialView(AppTheme.ContentNothing);
            }
        }
    }
}
