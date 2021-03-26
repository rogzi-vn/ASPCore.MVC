using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASPCoreMVC.Permissions;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using ASPCoreMVC.TCUEnglish.SkillCategories;
using ASPCoreMVC.TCUEnglish.SkillParts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPCoreMVC.Web.Pages.Manager.SkillParts
{
    [Authorize(ASPCoreMVCPermissions.ExamManager.Default)]
    public class SkillPartsIndexModel : AppPageModel
    {
        private readonly IExamCategoryService _ExamCategoryService;
        private readonly ISkillCategoryService _SkillCategoryService;
        private readonly ISkillPartService _SkillPartService;

        public List<SkillPartBaseDTO> SkillParts { get; private set; }
        public ExamCategoryBaseDTO CurrentExamCategory { get; set; }
        public SkillCategoryBaseDTO CurrentSkillCategory { get; set; }

        public SkillPartsIndexModel(
            IExamCategoryService examCategoryService,
            ISkillCategoryService skillCategoryService,
            ISkillPartService skillPartService)
        {
            _ExamCategoryService = examCategoryService;
            _SkillCategoryService = skillCategoryService;
            _SkillPartService = skillPartService;
        }

        public async Task<IActionResult> OnGetAsync(Guid? exId, Guid? skillCatId)
        {
            if (exId == null || exId == Guid.Empty)
            {
                ToastError(L["Please select correct exam category"]);
                return Redirect($"/manager/exam-categories");
            }
            else if (skillCatId == null || skillCatId == Guid.Empty)
            {
                ToastError(L["Please select correct skill category"]);
                return Redirect($"/manager/exam-categories/{exId}/skill-categories");
            }
            else
            {
                var examCat = await _ExamCategoryService.GetSimpify(exId.Value);
                if (examCat.Success)
                {
                    CurrentExamCategory = examCat.Data;
                    var skCat = _SkillCategoryService.GetSimpify(skillCatId.Value);
                    if (skCat.Success)
                    {
                        CurrentSkillCategory = skCat.Data;
                        var res = _SkillPartService.GetBase(skillCatId.Value);
                        if (res.Success)
                        {
                            SkillParts = res.Data;
                            return Page();
                        }
                        else
                        {
                            ToastError(res.Message);
                        }
                    }
                    else
                    {
                        ToastError(skCat.Message);
                    }
                    return Redirect($"/manager/exam-categories/{exId}/skill-categories");
                }
                ToastError(examCat.Message);
                return Redirect($"/manager/exam-categories");
            }
        }
    }
}
