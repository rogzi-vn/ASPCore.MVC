using ASPCoreMVC.AppUsers;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using ASPCoreMVC.TCUEnglish.ExamCatInstructors;
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

namespace ASPCoreMVC.Web.Pages.Dictionary.ExamCatInstructors.Partials
{
    [Route("/manager/exam-cate-instructors")]
    public class PartialsController : AbpController
    {
        private readonly IExamCategoryService _ExamCategoryService;
        private readonly IExamCatInstructService _ExamCatInstructService;

        private static string TableView = "~/Pages/Manager/ExamCatInstructors/Partials/Table.cshtml";
        private static string UserTableView = "~/Pages/Manager/ExamCatInstructors/Partials/UsersSelectorList.cshtml";

        public PartialsController(
            IExamCategoryService _ExamCategoryService,
            IExamCatInstructService _ExamCatInstructService)
        {
            this._ExamCatInstructService = _ExamCatInstructService;
            this._ExamCategoryService = _ExamCategoryService;
        }

        [Route("display/{examCategoryId:Guid}")]
        [HttpGet]
        public async Task<IActionResult> GetAsync(
            Guid examCategoryId,
            [FromQuery] int? p = 1,
            [FromQuery] string filter = "")
        {
            if (examCategoryId == Guid.Empty)
                return PartialView(AppTheme.ContentNothing);
            var examCat = await _ExamCategoryService.GetSimpify(examCategoryId);
            if (examCat.Success)
                ViewBag.CurrentExamCategory = examCat.Data;
            else
                return PartialView(AppTheme.ContentNothing);

            if (p == null || p <= 0) p = 1;
            ViewBag.p = p.Value;

            var serchInp = new GetExamCatInstructDTO
            {
                ExamCategoryId = examCategoryId,
                FilterDisplayName = filter ?? "",
                MaxResultCount = AppTheme.Limit,
                SkipCount = (p.Value - 1) * AppTheme.Limit,
            };
            var res = await _ExamCatInstructService.GetListAsync(serchInp);

            if (res.Success)
            {
                PagedResultDto<ExamCatInstructDTO> Containers = res.Data;

                string listRes = string.Format("Showing {0} to {1} of {2} entries",
                    res.Data.TotalCount > 0 ? serchInp.SkipCount + 1 : 0, serchInp.SkipCount + res.Data.Items.Count, res.Data.TotalCount);
                if (!filter.IsNullOrEmpty())
                {
                    listRes += string.Format(" for \"{0}\"", serchInp.FilterDisplayName);
                }
                ViewBag.ListState = listRes;

                ViewBag.Filter = filter;
                ViewBag.Pagination = PaginateHelper.Generate(
                    "javascript:syncVt('" + examCategoryId + "','{0}', '" + filter + "');",
                    p.Value, Containers.TotalCount, AppTheme.Limit);
                return PartialView(TableView, Containers);
            }
            else
            {
                return PartialView(AppTheme.ContentNothing);
            }
        }

        [Route("search-instructor/{examCategoryId:Guid}")]
        [HttpGet]
        public async Task<IActionResult> GetSearchUserAsync(
            Guid examCategoryId,
            [FromQuery] int? p = 1,
            [FromQuery] string filter = "")
        {
            if (examCategoryId == Guid.Empty)
                return PartialView(AppTheme.ContentNothing);
            var examCat = await _ExamCategoryService.GetSimpify(examCategoryId);
            if (examCat.Success)
                ViewBag.CurrentExamCategory = examCat.Data;
            else
                return PartialView(AppTheme.ContentNothing);

            if (p == null || p <= 0) p = 1;
            ViewBag.p = p.Value;

            var serchInp = new GetAppUserDTO
            {
                Filter = filter ?? "",
                MaxResultCount = AppTheme.Limit,
                SkipCount = (p.Value - 1) * AppTheme.Limit,
            };
            var res = await _ExamCatInstructService.GetInstructorsAsync(examCategoryId, serchInp);

            if (res.Success)
            {
                PagedResultDto<AppUserDTO> Containers = res.Data;

                string listRes = string.Format("Showing {0} to {1} of {2} entries",
                    res.Data.TotalCount > 0 ? serchInp.SkipCount + 1 : 0, serchInp.SkipCount + res.Data.Items.Count, res.Data.TotalCount);
                if (!filter.IsNullOrEmpty())
                {
                    listRes += string.Format(" for \"{0}\"", serchInp.Filter);
                }
                ViewBag.ListState = listRes;

                ViewBag.Filter = filter;
                ViewBag.Pagination = PaginateHelper.Generate(
                    "javascript:syncVt('" + examCategoryId + "','{0}', '" + filter + "');",
                    p.Value, Containers.TotalCount, AppTheme.Limit);
                return PartialView(UserTableView, Containers);
            }
            else
            {
                return PartialView(AppTheme.ContentNothing);
            }
        }

    }
}
