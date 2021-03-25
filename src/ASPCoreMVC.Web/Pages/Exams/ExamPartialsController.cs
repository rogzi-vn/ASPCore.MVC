using ASPCoreMVC.Common;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using ASPCoreMVC.TCUEnglish.ExamQuestionContainers;
using ASPCoreMVC.TCUEnglish.ExamQuestionGroups;
using ASPCoreMVC.TCUEnglish.ExamQuestions;
using ASPCoreMVC.TCUEnglish.SkillCategories;
using ASPCoreMVC.TCUEnglish.SkillParts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.Web.Pages.Exams
{
    [Authorize]
    [Route("exams/partials/exam-partials")]
    public class ExamPartialsController : Controller
    {
        private readonly IExamCategoryService _ExamCategoryServices;
        private readonly ISkillCategoryService _SkillCategoryServices;
        private readonly ISkillPartService _SkillPartServices;
        private readonly IRepository<ExamQuestionContainer, Guid> _ExamQuestionContainerRepository;
        private readonly IRepository<ExamQuestion, Guid> _ExamQuestionRepository;

        public ExamPartialsController(
            IExamCategoryService _ExamCategoryServices,
            ISkillCategoryService _SkillCategoryServices,
            ISkillPartService _SkillPartServices,
            IRepository<ExamQuestionContainer, Guid> _ExamQuestionContainerRepository,
            IRepository<ExamQuestion, Guid> _ExamQuestionRepository)
        {
            this._ExamCategoryServices = _ExamCategoryServices;
            this._SkillCategoryServices = _SkillCategoryServices;
            this._SkillPartServices = _SkillPartServices;
            this._ExamQuestionContainerRepository = _ExamQuestionContainerRepository;
            this._ExamQuestionRepository = _ExamQuestionRepository;
        }

        [HttpGet]
        [Route("redirect-to-data-manager/{skillPartId}")]
        public async Task<IActionResult> RedirectToDataManager(Guid skillPartId)
        {
            var skillPart = await _SkillPartServices.GetAsync(skillPartId);
            if (!skillPart.Success && skillPart.Data == null)
                return Redirect("/exams/exam-categories");
            var skillCat = await _SkillCategoryServices.GetAsync(skillPart.Data.ExamSkillCategoryId);
            if (!skillCat.Success && skillCat.Data == null)
                return Redirect("/exams/exam-categories");

            return Redirect($"/manager/exam-categories/{skillCat.Data.ExamCategoryId}/skill-categories/{skillCat.Data.Id}/skill-parts/{skillPart.Data.Id}/exam-data-libraries");
        }

        [HttpGet]
        [Route("transcript/{questionId:Guid}")]
        public async Task<IActionResult> TranscriptLoader(Guid questionId)
        {
            var currentQuestion = await _ExamQuestionRepository.GetAsync(questionId);
            if (currentQuestion != null)
            {
                ViewBag.Title = currentQuestion.Text;
                var currentContainer = await _ExamQuestionContainerRepository.GetAsync(currentQuestion.ExamQuestionContainerId.Value);
                if (currentContainer != null)
                {
                    ViewBag.Content = currentContainer.Article;
                    return PartialView("~/Pages/Exams/Partials/TranscriptDisplay.cshtml");
                }
            }

            return PartialView(AppTheme.ContentNothing);
        }

        [HttpGet]
        [Route("{type}/{id:Guid}/detail")]
        public async Task<IActionResult> Detail(HelperTipsTypes type,
            Guid id,
            [FromQuery(Name = "ex")] string ex,
            [FromQuery(Name = "sc")] string sc,
            [FromQuery(Name = "instructor")] Guid? instructor)
        {
            // Mã của dối tượng hiện tại
            ViewBag.Id = id;
            // Cho biết có hiển thị nút cập nhật dữ liệu hay không
            ViewBag.ShowManagerButton = type == HelperTipsTypes.SkillPart;

            var renderType = -1;

            if (type == HelperTipsTypes.Exam)
            {
                var exam = (await _ExamCategoryServices.GetAsync(id)).Data;
                ViewBag.ModalName = $"{exam.Name} Exam's tips";
                ViewBag.ModalContent = exam.Tips;
                renderType = (int)RenderExamTypes.Synthetic;
            }
            else if (type == HelperTipsTypes.SkillCategory)
            {
                var sCat = (await _SkillCategoryServices.GetAsync(id)).Data;
                var skPs = _SkillPartServices.GetBase(sCat.Id).Data;
                if (skPs.Count <= 1)
                {
                    ViewBag.ShowManagerButton = true;
                    ViewBag.Id = skPs.FirstOrDefault()?.Id ?? Guid.Empty;
                }
                ViewBag.ModalName = $"{sCat.Name} of {ex} Exam's tips";
                ViewBag.ModalContent = sCat.Tips;
                renderType = (int)RenderExamTypes.SkillCategory;
            }
            else if (type == HelperTipsTypes.SkillPart)
            {
                var sPart = (await _SkillPartServices.GetAsync(id)).Data;
                ViewBag.ModalName = $"{sc} {sPart.Name} of {ex} Exam's tips";
                ViewBag.ModalContent = sPart.Tips;
                renderType = (int)RenderExamTypes.SkillPart;
            }
            ViewBag.StartExamUrl = $"/exams/{renderType}/exam/{id}?instructor={instructor}";
            return PartialView("~/Pages/Exams/Partials/Detail.cshtml");
        }
    }
}
