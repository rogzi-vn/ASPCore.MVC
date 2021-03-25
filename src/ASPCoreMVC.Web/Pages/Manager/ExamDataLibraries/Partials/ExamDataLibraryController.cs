using ASPCoreMVC.Common;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using ASPCoreMVC.TCUEnglish.ExamDataLibraries;
using ASPCoreMVC.TCUEnglish.ExamQuestionContainers;
using ASPCoreMVC.TCUEnglish.ExamQuestionGroups;
using ASPCoreMVC.TCUEnglish.ExamQuestions;
using ASPCoreMVC.TCUEnglish.SkillCategories;
using ASPCoreMVC.TCUEnglish.SkillParts;
using ASPCoreMVC.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.Web.Pages.Manager.ExamDataLibraries.Partials
{
    [Authorize]
    [Route("/manager/exam-data-libraries")]
    public class ExamDataLibraryController : AbpController
    {
        private readonly IExamCategoryService _ExamCategoryService;
        private readonly ISkillCategoryService _SkillCategoryService;
        private readonly ISkillPartService _SkillPartService;

        private readonly IExamDataLibraryService _ExamDataLibraryService;

        private readonly IExamQuestionGroupService _ExamQuestionGroupService;

        private readonly IRepository<ExamQuestionContainer, Guid> _ExamQuestionContainerRepository;
        private readonly IRepository<ExamQuestion, Guid> _ExamQuestionRepository;

        public ExamDataLibraryController(
            IExamCategoryService examCategoryService,
            ISkillCategoryService skillCategoryService,
            ISkillPartService skillPartService,
            IExamDataLibraryService examDataLibraryService,
            IExamQuestionGroupService examQuestionGroupService,
            IRepository<ExamQuestionContainer, Guid> _ExamQuestionContainerRepository,
            IRepository<ExamQuestion, Guid> _ExamQuestionRepository)
        {
            _ExamCategoryService = examCategoryService;
            _SkillCategoryService = skillCategoryService;
            _SkillPartService = skillPartService;
            _ExamDataLibraryService = examDataLibraryService;
            _ExamQuestionGroupService = examQuestionGroupService;
            this._ExamQuestionContainerRepository = _ExamQuestionContainerRepository;
            this._ExamQuestionRepository = _ExamQuestionRepository;
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
                    return Content(currentContainer.Article);
                }
            }

            return Content("");
        }

        [Route("questions/{id}/preview")]
        [HttpGet]
        public async Task<IActionResult> GetQuestionPreviewAsync(
            Guid id)
        {
            var res = await _ExamDataLibraryService.GetForUpdateAsync(id);
            if (res.Success && res.Data != null)
            {
                ViewBag.RenderType = RenderExamTypes.SkillPart;
                var skillPart = (await _SkillPartService.GetAsync(res.Data.SkillPartId)).Data;
                ViewBag.Spk = skillPart;
                return PartialView("~/Partials/_Exam.QuestionDisplay.cshtml", res.Data);
            }
            else
                return PartialView(AppTheme.ContentNothing);
        }

        [Route("{skillPartId}/questions")]
        [HttpGet]
        public async Task<IActionResult> GetQuestionsAsync(
            Guid? skillPartId,
            [FromQuery] Guid? groupId,
            [FromQuery] int? p = 1,
            [FromQuery] string filter = "")
        {
            if (p == null || p <= 0) p = 1;
            ViewBag.p = p.Value;

            var serchInp = new GetExamContainerDTO
            {
                Filter = filter ?? "",
                MaxResultCount = AppTheme.Limit,
                SkipCount = (p.Value - 1) * AppTheme.Limit,
                SkillPartId = skillPartId.Value,
                QuestionGroupId = groupId
            };
            var res = await _ExamDataLibraryService.GetListAsync(serchInp);
            if (groupId == null || groupId == Guid.Empty)
            {
                ViewBag.GroupName = L["All"];
            }
            else
            {
                var qg = await _ExamQuestionGroupService.GetAsync(groupId.Value);
                if (!qg.Success || qg.Data == null)
                    ViewBag.GroupName = L["Unknow"];
                else
                    ViewBag.GroupName = qg.Data.Name;
            }
            if (res.Success)
            {
                PagedResultDto<ExamQuestionContainerDTO> Containers = res.Data;

                string listRes = string.Format("Showing {0} to {1} of {2} entries",
                    res.Data.TotalCount > 0 ? serchInp.SkipCount + 1 : 0, serchInp.SkipCount + res.Data.Items.Count, res.Data.TotalCount);
                if (!filter.IsNullOrEmpty())
                {
                    listRes += string.Format(" for \"{0}\"", serchInp.Filter);
                }
                ViewBag.ListState = listRes;

                ViewBag.Filter = filter;
                ViewBag.Pagination = PaginateHelper.Generate(
                    "javascript:syncQuestions('" + skillPartId + "', '{0}', '" + filter + "', '" + groupId + "');",
                    p.Value, Containers.TotalCount, AppTheme.Limit);
                return PartialView("~/Pages/Manager/ExamDataLibraries/Partials/Questions.cshtml", Containers);
            }
            else
            {
                return PartialView(AppTheme.ContentNothing);
            }
        }

        [Route("{skillPartId}/question-groups")]
        [HttpGet]
        public async Task<IActionResult> GetQuestionGroupsAsync(
            Guid? skillPartId,
            [FromQuery] int? p = 1,
            [FromQuery] string filter = "")
        {
            if (p == null || p <= 0) p = 1;
            ViewBag.p = p.Value;

            var serchInp = new GetQuestionGroupDTO
            {
                Filter = filter ?? "",
                MaxResultCount = AppTheme.Limit,
                SkipCount = (p.Value - 1) * AppTheme.Limit,
                SkillPartId = skillPartId.Value
            };
            var res = await _ExamQuestionGroupService.GetListAsync(serchInp);
            if (res.Success)
            {
                PagedResultDto<QuestionGroupDTO> Containers = res.Data;

                string listRes = string.Format("Showing {0} to {1} of {2} entries",
                    res.Data.TotalCount > 0 ? serchInp.SkipCount + 1 : 0, serchInp.SkipCount + res.Data.Items.Count, res.Data.TotalCount);
                if (!filter.IsNullOrEmpty())
                {
                    listRes += string.Format(" for \"{0}\"", serchInp.Filter);
                }
                ViewBag.ListState = listRes;

                ViewBag.Filter = filter;
                ViewBag.Pagination = PaginateHelper.Generate(
                    "javascript:syncQuestionGroups('" + skillPartId + "', '{0}', '" + filter + "');",
                    p.Value, Containers.TotalCount, AppTheme.Limit);
                return PartialView("~/Pages/Manager/ExamDataLibraries/Partials/QuestionGroups.cshtml", Containers);
            }
            else
            {
                return PartialView(AppTheme.ContentNothing);
            }
        }
    }
}
