using ASPCoreMVC.Common;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using ASPCoreMVC.TCUEnglish.SkillCategories;
using ASPCoreMVC.TCUEnglish.SkillParts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace ASPCoreMVC.Web.Pages._Common.Partials
{
    [Authorize]
    [Route("/exam/helper")]
    public class TipsPartialController : AbpController
    {
        private readonly IExamCategoryService _ExamCategoryServices;
        private readonly ISkillCategoryService _SkillCategoryServices;
        private readonly ISkillPartService _SkillPartServices;

        public TipsPartialController(
            IExamCategoryService _ExamCategoryServices,
            ISkillCategoryService _SkillCategoryServices,
            ISkillPartService _SkillPartServices)
        {
            this._ExamCategoryServices = _ExamCategoryServices;
            this._SkillCategoryServices = _SkillCategoryServices;
            this._SkillPartServices = _SkillPartServices;
        }

        [HttpGet]
        [Route("tips/{type}")]
        public async Task<IActionResult> Detail(HelperTipsTypes type,
            [FromQuery] Guid id,
            [FromQuery] bool testable = false)
        {
            var modalTitle = "";
            var modalContent = "";
            var examStartPath = "";

            if (type == HelperTipsTypes.Exam)
            {
                var exam = (await _ExamCategoryServices.GetAsync(id)).Data;
                modalTitle = String.Format(L["{0} tips"], exam.Name);
                modalContent = exam.Tips;
            }
            else if (type == HelperTipsTypes.SkillCategory)
            {
                var sCat = (await _SkillCategoryServices.GetAsync(id)).Data;
                var exam = (await _ExamCategoryServices.GetAsync(sCat.ExamCategoryId)).Data;
                modalTitle = String.Format(L["{0}:{1} tips"], exam.Name, sCat.Name);
                modalContent = sCat.Tips;
            }
            else if (type == HelperTipsTypes.SkillPart)
            {
                var sPart = (await _SkillPartServices.GetAsync(id)).Data;
                var sCat = (await _SkillCategoryServices.GetAsync(sPart.ExamSkillCategoryId)).Data;
                var exam = (await _ExamCategoryServices.GetAsync(sCat.ExamCategoryId)).Data;

                modalTitle = String.Format(L["{0}:{1} {2} tips"], exam.Name, sCat.Name, sPart.Name);
                modalContent = sPart.Tips;
            }
            ViewBag.ModalTitle = modalTitle;
            ViewBag.ModalContent = modalContent;
            ViewBag.ExamStartPath = examStartPath;
            return PartialView("~/Pages/_Common/Partials/Tips.cshtml");
        }
    }
}
