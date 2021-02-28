using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCoreMVC._Commons;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using ASPCoreMVC.TCUEnglish.SkillCategories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPCoreMVC.Web.Pages.Manager.SkillCategories.Partials
{
    public class SkillCategoryCreateUpdateModel : AppPageModel
    {
        private readonly IExamCategoryService _ExamCategoryService;
        private readonly ISkillCategoryService _SkillCategoryService;

        [BindProperty]
        public CreateUpdateSkillCategoryDTO SkillCategoryDTO { get; set; }
        public ExamCategoryBaseDTO CurrentExamCategory { get; set; }

        public SkillCategoryCreateUpdateModel(IExamCategoryService examCategoryService,
            ISkillCategoryService _SkillCategoryService)
        {
            _ExamCategoryService = examCategoryService;
            this._SkillCategoryService = _SkillCategoryService;
        }

        private async Task<string> PreLaunch(Guid? exCatId, Guid? id)
        {
            if (exCatId == null || exCatId == Guid.Empty)
            {
                ToastError(L["Please select correct Exam category"]);
                return $"/manager/exam-categories";
            }

            var examCat = await _ExamCategoryService.GetSimpify(exCatId.Value);
            if (!examCat.Success)
            {
                ToastError(examCat.Message);
                return $"/manager/exam-categories";
            }
            CurrentExamCategory = examCat.Data;

            return null;
        }

        public async Task<IActionResult> OnGetAsync(Guid? exCatId, Guid? id)
        {
            var redirectPath = await PreLaunch(exCatId, id);
            if (redirectPath != null)
            {
                if (redirectPath == "")
                    return Page();
                else
                    return Redirect(redirectPath);
            }

            /* Main code */

            if (id == null || id == Guid.Empty)
            {
                SkillCategoryDTO = new CreateUpdateSkillCategoryDTO
                {
                    ExamCategoryId = exCatId.Value
                };
                return Page();
            }

            var skillCat = await _SkillCategoryService.GetDataForUpdate(id.Value);
            if (skillCat.Success)
            {
                SkillCategoryDTO = skillCat.Data;
                return Page();
            }

            ToastError(skillCat.Message);
            return Redirect($"/manager/exam-categories/{CurrentExamCategory.Id}/skill-categories");
        }

        public async Task<IActionResult> OnPostAsync(Guid? exCatId, Guid? id)
        {
            var redirectPath = await PreLaunch(exCatId, id);
            if (redirectPath != null)
            {
                if (redirectPath == "")
                    return Page();
                else
                    return Redirect(redirectPath);
            }

            /* Main code */
            if (!ModelState.IsValid)
            {
                ToastError(L["Your input is invalid"]);
                return Page();
            }

            ResponseWrapper<SkillCategoryDTO> res;
            if (SkillCategoryDTO.Id != Guid.Empty)
                res = await _SkillCategoryService.UpdateAsync(SkillCategoryDTO.Id, SkillCategoryDTO);
            else
                res = await _SkillCategoryService.CreateAsync(SkillCategoryDTO);

            if (res != null)
            {
                ToastSuccess(res.Message);
                return Redirect($"/manager/exam-categories/{res.Data.ExamCategoryId}/skill-categories");
            }

            ToastError(res.Message);
            return Page();
        }
    }
}
