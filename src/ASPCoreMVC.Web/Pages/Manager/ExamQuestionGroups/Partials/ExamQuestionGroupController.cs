using ASPCoreMVC.TCUEnglish.ExamQuestionGroups;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ASPCoreMVC.Web.Pages.Manager.ExamQuestionGroups.Partials
{
    [Route("/manager/question-groups")]
    public class ExamQuestionGroupController : Controller
    {
        private readonly IExamQuestionGroupService _ExamQuestionGroupService;
        public ExamQuestionGroupController(
            IExamQuestionGroupService _ExamQuestionGroupService)
        {
            this._ExamQuestionGroupService = _ExamQuestionGroupService;
        }

        [Route("{skillPartId}/create")]
        [HttpGet]
        public IActionResult CreateQuestionGroups(Guid skillPartId)
        {

            return PartialView("~/Pages/Manager/ExamQuestionGroups/Partials/CreateUpdateQuestionGroup.cshtml",
                new QuestionGroupDTO
                {
                    SkillPartId = skillPartId
                });
        }

        [Route("update/{id}")]
        [HttpGet]
        public async Task<IActionResult> SyncQuestionGroups(Guid id)
        {
            var res = await _ExamQuestionGroupService.GetAsync(id);
            if (!res.Success || res.Data == null)
                return PartialView(AppTheme.ContentNothing);

            return PartialView("~/Pages/Manager/ExamQuestionGroups/Partials/CreateUpdateQuestionGroup.cshtml", res.Data);
        }
    }
}
