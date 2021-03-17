using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using ASPCoreMVC.TCUEnglish.ExamDataLibraries;
using ASPCoreMVC.TCUEnglish.SkillCategories;
using ASPCoreMVC.TCUEnglish.SkillParts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.Web.Pages.Manager.ExamDataLibraries
{
    [Authorize]
    public class ExamDataLibrariesIndexModel : AppPageModel
    {
        private readonly IExamCategoryService _ExamCategoryService;
        private readonly ISkillCategoryService _SkillCategoryService;
        private readonly ISkillPartService _SkillPartService;

        private readonly IExamDataLibraryService _ExamDataLibraryService;
        public PagedResultDto<ExamQuestionContainerDTO> Containers { get; private set; }
        public ExamCategoryBaseDTO CurrentExamCategory { get; set; }
        public SkillCategoryBaseDTO CurrentSkillCategory { get; set; }
        public SkillPartBaseDTO CurrentSkillPart { get; set; }

        public ExamDataLibrariesIndexModel(
            IExamCategoryService examCategoryService,
            ISkillCategoryService skillCategoryService,
            ISkillPartService skillPartService,
            IExamDataLibraryService examDataLibraryService)
        {
            _ExamCategoryService = examCategoryService;
            _SkillCategoryService = skillCategoryService;
            _SkillPartService = skillPartService;
            _ExamDataLibraryService = examDataLibraryService;
        }

        public async Task<IActionResult> OnGetAsync(
            Guid? exId,
            Guid? skillCatId,
            Guid? skillPartId)
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
            else if (skillPartId == null || skillPartId == Guid.Empty)
            {
                ToastError(L["Please select correct skill part"]);
                return Redirect($"/manager/exam-categories/{exId}/skill-categories/{skillCatId}/skill-parts");
            }
            else
            {
                // Lấy exam category
                var examCat = await _ExamCategoryService.GetSimpify(exId.Value);
                if (examCat.Success)
                {
                    CurrentExamCategory = examCat.Data;
                    // Lấy skill category
                    var skCat = _SkillCategoryService.GetSimpify(skillCatId.Value);
                    if (skCat.Success)
                    {
                        CurrentSkillCategory = skCat.Data;
                        // Lấy skill part
                        var skPart = _SkillPartService.GetSimpify(skillPartId.Value);
                        if (skPart.Success)
                        {
                            CurrentSkillPart = skPart.Data;
                            return Page();
                        }
                        else
                        {
                            ToastError(skPart.Message);
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
