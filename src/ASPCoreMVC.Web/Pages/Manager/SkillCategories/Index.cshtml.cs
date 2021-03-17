using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using ASPCoreMVC.TCUEnglish.SkillCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPCoreMVC.Web.Pages.Manager.SkillCategories
{
    [Authorize]
    public class SkillCategoriesIndexModel : AppPageModel
    {
        private readonly IExamCategoryService _ExamCategoryService;
        private readonly ISkillCategoryService _SkillCategoryService;

        public List<SkillCategoryBaseDTO> SkillCategories { get; private set; }
        public ExamCategoryBaseDTO CurrentExamCategory { get; set; }

        public SkillCategoriesIndexModel(IExamCategoryService examCategoryService,
            ISkillCategoryService skillCategoryService)
        {
            _ExamCategoryService = examCategoryService;
            _SkillCategoryService = skillCategoryService;
        }
        public async Task<IActionResult> OnGetAsync(Guid? exCatId)
        {
            if (exCatId == null || exCatId == Guid.Empty)
            {
                ToastError(L["Please select correct Exam category"]);
                return Redirect($"/manager/exam-categories");
            }
            else
            {
                var examCat = await _ExamCategoryService.GetSimpify(exCatId.Value);
                if (examCat.Success)
                {
                    CurrentExamCategory = examCat.Data;
                    var res = _SkillCategoryService.GetBase(exCatId.Value);
                    if (res.Success)
                    {
                        SkillCategories = res.Data;
                    }
                    else
                    {
                        ToastError(res.Message);
                    }
                    return Page();
                }
                else
                {
                    ToastError(examCat.Message);
                    return Redirect($"/manager/exam-categories");
                }
            }
        }
    }
}
