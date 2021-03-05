using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCoreMVC.Common;
using ASPCoreMVC.TCUEnglish.UserExams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPCoreMVC.Web.Pages.Exams
{
    public class ExamModel : AppPageModel
    {
        private readonly IRenderExamService _RenderExamService;

        public ExamModel(
            IRenderExamService _RenderExamService)
        {
            this._RenderExamService = _RenderExamService;
        }

        [BindProperty]
        public ExamForRenderDTO ExamContent { get; set; }

        public async Task<IActionResult> OnGetAsync(RenderExamTypes renderType, Guid destId)
        {
            var res = await _RenderExamService.GetRenderExam(renderType, destId);
            if (!res.Success || res.Data == null)
            {
                ToastError(res.Message);
                return Redirect("/exams/exam-categories");
            }

            ExamContent = res.Data;
            return Page();
        }
    }
}
