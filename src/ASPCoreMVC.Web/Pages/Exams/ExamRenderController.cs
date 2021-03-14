using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCoreMVC.AppUsers;
using ASPCoreMVC.Common;
using ASPCoreMVC.TCUEnglish.ExamLogs;
using ASPCoreMVC.TCUEnglish.UserExams;
using ASPCoreMVC.Web.Helpers;
using ASPCoreMVC.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Volo.Abp.AspNetCore.Mvc;

namespace ASPCoreMVC.Web.Pages.Exams.Partials
{
    public class ExamModel : AbpController
    {
        private readonly IRenderExamService _RenderExamService;
        private readonly IAppUserService _AppUserService;

        private readonly IExamLogService _ExamLogService;

        public ExamModel(
            IRenderExamService _RenderExamService,
            IAppUserService _AppUserService,
            IExamLogService _ExamLogService)
        {
            this._RenderExamService = _RenderExamService;
            this._AppUserService = _AppUserService;
            this._ExamLogService = _ExamLogService;
        }

        [HttpGet]
        [Route("/exams/preview/{logId:Guid}")]
        public async Task<IActionResult> RenderPreviewExam(Guid? logId)
        {
            if (logId == null || logId == Guid.Empty)
            {
                this.ToastError(L["Can not process your preview exam"]);
                return Redirect("/");
            }

            var res = await _ExamLogService.GetAsync(logId.Value);
            var model = new ExamRenderViewModel();
            if (res.Success)
            {
                model.ExamContent = JsonConvert.DeserializeObject<ExamForRenderDTO>(res.Data.RawExamRendered);
                model.ExamLogId = res.Data.Id;

                model = await ProcessParams(model);
                ViewBag.Answers = JsonConvert.DeserializeObject<List<QAPairDTO>>(res.Data.UserAnswers);
                ViewBag.Scores = res.Data.ExamScores.ToString("0.0");

                ViewBag.TimeInMinutes = res.Data.ExamTimeInMinutes;

                return View(AppTheme.ExamPreview, model);
            }
            else
            {
                this.ToastError(L["Can not process your previous exam"]);
                return Redirect("/");
            }
        }

        [HttpGet]
        [Route("/exams/re-work/{logId:Guid}")]
        public async Task<IActionResult> RenderReWorkExam(Guid? logId)
        {
            if (logId == null || logId == Guid.Empty)
            {
                this.ToastError(L["Can not process your previous exam"]);
                return Redirect("/");
            }

            var res = await _ExamLogService.GetAsync(logId.Value);
            var model = new ExamRenderViewModel();
            if (res.Success)
            {
                model.ExamContent = JsonConvert.DeserializeObject<ExamForRenderDTO>(res.Data.RawExamRendered);
                model.ExamLogId = res.Data.Id;

                if (!res.Data.UserAnswers.IsNullOrEmpty())
                {
                    // Prevent user re work done exam
                    return Redirect($"/exams/preview/{logId}");
                }

                model = await ProcessParams(model);
                return View(AppTheme.ExamContainer, model);
            }
            else
            {
                this.ToastError(L["Can not process your previous exam"]);
                return Redirect("/");
            }
        }

        [HttpGet]
        [Route("/exams/{renderType:int}/exam/{destId:Guid}")]
        public async Task<IActionResult> RenderExam(
            RenderExamTypes renderType,
            Guid destId,
            [FromQuery(Name = "instructor")] Guid? instructor)
        {
            // Check if user have other test
            var previousLogId = _ExamLogService.GetLastExamNotFinished();
            if (previousLogId != null && previousLogId != Guid.Empty)
            {
                // Ngắn chặn việc lặp lại tạo bài test
                return Redirect($"/exams/re-work/{previousLogId}");
            }

            var res = await _RenderExamService.GetRenderExam(renderType, destId);
            if (!res.Success || res.Data == null)
            {
                this.ToastError(res.Message);
                return Redirect("/exams/exam-categories");
            }

            var model = new ExamRenderViewModel();

            model.ExamContent = res.Data;

            // Lưu nhật ký bài thi lại
            model.ExamLogId = await SaveExamLogs(model.ExamContent, destId, instructor);

            if (model.ExamLogId == null || model.ExamLogId == Guid.Empty)
            {
                this.ToastError(L["Can not save your exam, please try again"]);
                return Redirect("/exams/exam-categories");
            }

            model = await ProcessParams(model);

            return View(AppTheme.ExamContainer, model);
        }

        private async Task<ExamRenderViewModel> ProcessParams(ExamRenderViewModel inp)
        {
            inp.ExamUser = (await _AppUserService.GetAsync(CurrentUser.Id.Value)).Data;
            if (inp.ExamUser.DisplayName.IsNullOrEmpty())
                inp.ExamUser.DisplayName = inp.ExamUser.UserName;

            #region Exact exam name
            if (inp.ExamContent.RenderExamType == RenderExamTypes.SkillPart)
                inp.ExamName = inp.ExamContent.SkillCategories.FirstOrDefault()?.SkillParts?.FirstOrDefault()?.Name ?? "";
            else if (inp.ExamContent.RenderExamType == RenderExamTypes.SkillCategory)
                inp.ExamName = inp.ExamContent.SkillCategories.FirstOrDefault()?.Name ?? "";
            else
                inp.ExamName = inp.ExamContent.Name;
            inp.ExamName = string.Format("{0}'s testing", inp.ExamName);
            #endregion

            #region Exact time
            if (inp.ExamContent.RenderExamType == RenderExamTypes.SkillPart)
                inp.LimitTime = inp.ExamContent.SkillCategories.FirstOrDefault()?.SkillParts?.FirstOrDefault().LimitTimeInMinutes ?? 10F;
            else if (inp.ExamContent.RenderExamType == RenderExamTypes.SkillCategory)
                inp.LimitTime = inp.ExamContent.SkillCategories.FirstOrDefault()?.LimitTimeInMinutes ?? 20F;
            else
                inp.LimitTime = inp.ExamContent.SkillCategories.Sum(x => x.LimitTimeInMinutes);
            #endregion

            return inp;
        }

        private async Task<Guid?> SaveExamLogs(ExamForRenderDTO examRaw, Guid destId, Guid? instructor)
        {
            var examLog = new ExamLogDTO
            {
                RenderExamType = examRaw.RenderExamType,
                Name = examRaw.Name,
                RawExamRendered = JsonConvert.SerializeObject(examRaw),
                ExamCatInstructorId = instructor,
                DestinationId = destId,
                ExamCategoryId = examRaw.Id
            };

            var res = await _ExamLogService.CreateAsync(examLog);
            if (res.Success)
            {
                return res.Data.Id;
            }
            else
            {
                return null;
            }
        }
    }
}
