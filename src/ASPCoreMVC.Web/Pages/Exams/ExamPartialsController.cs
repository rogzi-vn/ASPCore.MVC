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
    [Route("exam/partials/exam-partials")]
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
            //// Mã của dối tượng hiện tại
            //ViewBag.Id = id;
            //// Cho biết có hiển thị nút cập nhật dữ liệu hay không
            //ViewBag.ShowManagerButton = type == DisplayDetailTypes.SkillPart;

            //if (type == DisplayDetailTypes.Exam)
            //{
            //    var exam = (await _ExamCategoryServices.GetAsync(id)).Data;
            //    ViewBag.ModalName = $"{exam.Name} Exam's tips";
            //    ViewBag.ModalContent = exam.About;
            //    ViewBag.StartExamUrl = "/";
            //}
            //else if (type == DisplayDetailTypes.SkillCategory)
            //{
            //    var sCat = (await _SkillCategoryServices.GetAsync(id)).Data;
            //    ViewBag.ModalName = $"{sCat.Name} of {ex} Exam's tips";
            //    ViewBag.ModalContent = sCat.SummaryDescription;
            //    ViewBag.StartExamUrl = "/";
            //}
            //else if (type == DisplayDetailTypes.SkillPart)
            //{
            //    var sPart = (await _SkillPartServices.GetAsync(id)).Data;
            //    ViewBag.ModalName = $"{sc} {sPart.Name} of {ex} Exam's tips";
            //    ViewBag.ModalContent = sPart.Tips;
            //    ViewBag.StartExamUrl = "/";
            //}
            return PartialView("~/Pages/Exams/Partials/Detail.cshtml");
        }
    }
}
