using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCoreMVC.Permissions;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPCoreMVC.Web.Pages.Manager.ExamCatInstructors
{
    [Authorize(ASPCoreMVCPermissions.ExamManager.Default)]
    public class IndexModel : AppPageModel
    {
        private readonly IExamCategoryService _ExamCategoryService;
        public ExamCategoryBaseDTO CurrentExamCategory { get; set; }

        public IndexModel(IExamCategoryService _ExamCategoryService)
        {
            this._ExamCategoryService = _ExamCategoryService;
        }
        public async Task<IActionResult> OnGet(Guid? exCatId)
        {
            if (exCatId == null || exCatId == Guid.Empty)
            {
                ToastError(L["Please select correct Exam category"]);
                return Redirect($"/manager/exam-categories");
            }
            else
            {
                var examCat = await _ExamCategoryService.GetSimpify(exCatId.Value);
                if (examCat.Success)
                {
                    CurrentExamCategory = examCat.Data;
                    return Page();
                }
                else
                {
                    ToastError(examCat.Message);
                    return Redirect($"/manager/exam-categories");
                }
            }
        }
    }
}
