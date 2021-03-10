using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCoreMVC.AppUsers;
using ASPCoreMVC.Common;
using ASPCoreMVC.TCUEnglish.UserExams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPCoreMVC.Web.Pages.Exams
{
    public class ExamModel : AppPageModel
    {
        private readonly IRenderExamService _RenderExamService;
        private readonly IAppUserService _AppUserService;

        public ExamModel(
            IRenderExamService _RenderExamService,
            IAppUserService _AppUserService)
        {
            this._RenderExamService = _RenderExamService;
            this._AppUserService = _AppUserService;
        }

        [BindProperty]
        public ExamForRenderDTO ExamContent { get; set; }

        public AppUserDTO ExamUser { get; set; }
        public string ExamName { get; set; } = "";
        public float LimitTime { get; set; } = 0F;

        public async Task<IActionResult> OnGetAsync(RenderExamTypes renderType, Guid destId)
        {
            var res = await _RenderExamService.GetRenderExam(renderType, destId);
            if (!res.Success || res.Data == null)
            {
                ToastError(res.Message);
                return Redirect("/exams/exam-categories");
            }

            ExamUser = (await _AppUserService.GetAsync(CurrentUser.Id.Value)).Data;
            if (ExamUser.DisplayName.IsNullOrEmpty())
                ExamUser.DisplayName = ExamUser.UserName;

            ExamContent = res.Data;

            #region Exact exam name
            if (ExamContent.RenderExamType == RenderExamTypes.SkillPart)
                ExamName = ExamContent.SkillCategories.FirstOrDefault()?.SkillParts?.FirstOrDefault()?.Name ?? "";
            else if (ExamContent.RenderExamType == RenderExamTypes.SkillCategory)
                ExamName = ExamContent.SkillCategories.FirstOrDefault()?.Name ?? "";
            else
                ExamName = ExamContent.Name;
            ExamName = string.Format("{0}'s testing", ExamName);
            #endregion

            #region Exact time
            if (ExamContent.RenderExamType == RenderExamTypes.SkillPart)
                LimitTime = ExamContent.SkillCategories.FirstOrDefault()?.SkillParts?.FirstOrDefault().LimitTimeInMinutes ?? 10F;
            else if (ExamContent.RenderExamType == RenderExamTypes.SkillCategory)
                LimitTime = ExamContent.SkillCategories.FirstOrDefault()?.LimitTimeInMinutes ?? 20F;
            else
                LimitTime = ExamContent.SkillCategories.Sum(x => x.LimitTimeInMinutes);
            #endregion

            return Page();
        }
    }
}
