using ASPCoreMVC.Common;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using ASPCoreMVC.TCUEnglish.SkillCategories;
using ASPCoreMVC.TCUEnglish.SkillParts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreMVC.Web.Pages.Exams
{
    [Route("exams/partials/exam-partials")]
    public class ExamPartialsController : Controller
    {
        private readonly IExamCategoryService _ExamCategoryServices;
        private readonly ISkillCategoryService _SkillCategoryServices;
        private readonly ISkillPartService _SkillPartServices;

        public ExamPartialsController(
            IExamCategoryService _ExamCategoryServices,
            ISkillCategoryService _SkillCategoryServices,
            ISkillPartService _SkillPartServices)
        {
            this._ExamCategoryServices = _ExamCategoryServices;
            this._SkillCategoryServices = _SkillCategoryServices;
            this._SkillPartServices = _SkillPartServices;
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
