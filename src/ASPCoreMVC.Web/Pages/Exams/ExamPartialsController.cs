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
        [Route("{type}/{id:Guid}/detail")]
        public async Task<IActionResult> Detail(HelperTipsTypes type, Guid id,
            [FromQuery(Name = "ex")] string ex,
            [FromQuery(Name = "sc")] string sc)
        {
            // Mã của dối tượng hiện tại
            ViewBag.Id = id;
            // Cho biết có hiển thị nút cập nhật dữ liệu hay không
            ViewBag.ShowManagerButton = type == HelperTipsTypes.SkillPart;

            if (type == HelperTipsTypes.Exam)
            {
                var exam = (await _ExamCategoryServices.GetAsync(id)).Data;
                ViewBag.ModalName = $"{exam.Name} Exam's tips";
                ViewBag.ModalContent = exam.Tips;
                ViewBag.StartExamUrl = $"/exams/{(int)RenderExamTypes.Synthetic}/exam/{id}";
            }
            else if (type == HelperTipsTypes.SkillCategory)
            {
                var sCat = (await _SkillCategoryServices.GetAsync(id)).Data;
                ViewBag.ModalName = $"{sCat.Name} of {ex} Exam's tips";
                ViewBag.ModalContent = sCat.Tips;
                ViewBag.StartExamUrl = $"/exams/{(int)RenderExamTypes.SkillCategory}/exam/{id}";
            }
            else if (type == HelperTipsTypes.SkillPart)
            {
                var sPart = (await _SkillPartServices.GetAsync(id)).Data;
                ViewBag.ModalName = $"{sc} {sPart.Name} of {ex} Exam's tips";
                ViewBag.ModalContent = sPart.Tips;
                ViewBag.StartExamUrl = $"/exams/{(int)RenderExamTypes.SkillPart}/exam/{id}";
            }
            return PartialView("~/Pages/Exams/Partials/Detail.cshtml");
        }
    }
}
