using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using ASPCoreMVC.TCUEnglish.ExamCatInstructors;
using ASPCoreMVC.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.Web.Pages.Instructors
{
    [Authorize]
    public class IndexModel : AppPageModel
    {
        private readonly IExamCatInstructService _ExamCatInstructService;
        private readonly IExamCategoryService _ExamCategoryService;

        public PagedResultDto<ExamCatInstructDTO> Instructors { get; set; }

        public string ListState { get; set; }
        [BindProperty]
        public string Filter { get; set; }
        [BindProperty]
        public Guid? ExamCategoryId { get; set; } = null;
        public string Pagination { get; set; }
        [BindProperty]
        public int CurrentPage { get; set; } = 1;
        public List<ExamCategoryBaseDTO> ExamCategories { get; set; }

        public IndexModel(
            IExamCatInstructService _ExamCatInstructService,
            IExamCategoryService _ExamCategoryService)
        {
            this._ExamCatInstructService = _ExamCatInstructService;
            this._ExamCategoryService = _ExamCategoryService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var res = await _ExamCategoryService.GetBase();
            if (res.Success && res.Data.Count > 0)
                ExamCategories = res.Data;
            else
            {
                ToastError(L["No exams"]);
                return Redirect("/");
            }

            if (ExamCategoryId == null)
            {
                ExamCategoryId = res.Data.FirstOrDefault()?.Id ?? Guid.Empty;
            }
            if (!await SyncUserInstructors())
                return Redirect("/");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var res = await _ExamCategoryService.GetBase();
            if (res.Success && res.Data.Count > 0)
                ExamCategories = res.Data;
            else
            {
                ToastError(L["No exams"]);
                return Redirect("/");
            }

            if (ExamCategoryId == null)
            {
                ExamCategoryId = res.Data.FirstOrDefault()?.Id ?? Guid.Empty;
            }
            if (!await SyncUserInstructors())
                return Redirect("/");
            return Page();
        }

        public async Task<bool> SyncUserInstructors()
        {
            if (ExamCategoryId == null || ExamCategoryId.Value == Guid.Empty)
            {
                ToastError(L["Exam category is invalid"]);
                return false;
            }

            if (CurrentPage <= 0) CurrentPage = 1;

            var searchInp = new GetExamCatInstructDTO
            {
                //UserId = null, // Không có phần này để có thể lấy tất cả giáo viên hướng dẫn
                ExamCategoryId = ExamCategoryId,
                FilterDisplayName = Filter,
                MaxResultCount = AppTheme.Limit,
                SkipCount = (CurrentPage - 1) * AppTheme.Limit,
            };

            var res = await _ExamCatInstructService.GetListAsync(searchInp);

            if (res.Success)
            {
                Instructors = res.Data;

                ListState = string.Format("Showing {0} to {1} of {2} entries",
                   res.Data.TotalCount > 0 ? searchInp.SkipCount + 1 : 0, searchInp.SkipCount + res.Data.Items.Count, res.Data.TotalCount);

                if (!Filter.IsNullOrEmpty())
                {
                    ListState += string.Format(" for \"{0}\"", searchInp.FilterDisplayName);
                }

                Pagination = PaginateHelper.Generate(
                    "javascript:movePage({0});",
                    CurrentPage, Instructors.TotalCount, AppTheme.Limit);

                return true;
            }
            else
            {
                ToastError(res.Message);
                return false;
            }
        }
    }
}
