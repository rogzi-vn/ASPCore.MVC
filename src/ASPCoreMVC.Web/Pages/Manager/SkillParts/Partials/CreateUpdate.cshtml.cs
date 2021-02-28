using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCoreMVC._Commons;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using ASPCoreMVC.TCUEnglish.ExamDataLibraries;
using ASPCoreMVC.TCUEnglish.SkillCategories;
using ASPCoreMVC.TCUEnglish.SkillParts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPCoreMVC.Web.Pages.Manager.SkillParts.Partials
{
    public class SkillPartCreateUpdateModel : AppPageModel
    {
        private readonly IExamCategoryService _ExamCategoryService;
        private readonly ISkillCategoryService _SkillCategoryService;
        private readonly ISkillPartService _SkillPartService;

        private readonly IExamDataLibraryService _ExamDataLibraryService;

        [BindProperty]
        public CreateUpdateSkillPartDTO SkillPartDTO { get; set; }
        [BindProperty]
        public bool IsEditable { get; set; } = true;
        public ExamCategoryBaseDTO CurrentExamCategory { get; set; }
        public SkillCategoryBaseDTO CurrentSkillCategory { get; set; }

        public SkillPartCreateUpdateModel(
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

        public async Task<string> PreLaunch(Guid? exId, Guid? skillCatId, Guid? id)
        {
            if (exId == null || exId == Guid.Empty)
            {
                ToastError(L["Please select correct exam category"]);
                return $"/manager/exam-categories";
            }

            if (skillCatId == null || skillCatId == Guid.Empty)
            {
                ToastError(L["Please select correct skill category"]);
                return $"/manager/exam-categories/{exId}/skill-categories";
            }

            // Lấy loại kỳ thi
            var examCat = await _ExamCategoryService.GetSimpify(exId.Value);
            if (!examCat.Success)
            {
                ToastError(examCat.Message);
                return $"/manager/exam-categories";
            }
            CurrentExamCategory = examCat.Data;

            // Lấy mục kỹ năng
            var skCat = _SkillCategoryService.GetSimpify(skillCatId.Value);
            if (!skCat.Success)
            {
                ToastError(skCat.Message);
                return $"/manager/exam-categories/{exId}/skill-categories";
            }
            CurrentSkillCategory = skCat.Data;

            return null;
        }

        public async Task<IActionResult> OnGetAsync(Guid? exId, Guid? skillCatId, Guid? id)
        {
            var redirectPath = await PreLaunch(exId, skillCatId, id);
            if (redirectPath != null)
            {
                if (redirectPath == "")
                    return Page();
                else
                    return Redirect(redirectPath);
            }

            /* Main code */

            if (id == Guid.Empty || id == null)
            {
                // Đây là trường hợp tạo
                SkillPartDTO = new CreateUpdateSkillPartDTO
                {
                    ExamSkillCategoryId = CurrentSkillCategory.Id
                };
                return Page();
            }

            // Đây là trường hợp cập nhật
            // Lấy phân mục của kỹ năng hiện tại
            var skPart = await _SkillPartService.GetDataForUpdate(id.Value);
            if (skPart.Success && skPart.Data != null)
            {
                // Kiểm tra xem có nên cho phép sửa không
                IsEditable = !(await _ExamDataLibraryService.GetIsHaveAnyAsync(skPart.Data.Id)).Data;
                // Đẩy về form
                SkillPartDTO = skPart.Data;
                return Page();
            }

            ToastError(skPart.Message);
            return Redirect($"/manager/exam-categories/{exId}/skill-categories");
        }

        public async Task<IActionResult> OnPostAsync(Guid? exId, Guid? skillCatId, Guid? id)
        {
            var redirectPath = await PreLaunch(exId, skillCatId, id);
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

            ResponseWrapper<SkillPartDTO> res;
            if (SkillPartDTO.Id != Guid.Empty)
                res = await _SkillPartService.UpdateAsync(SkillPartDTO.Id, SkillPartDTO);
            else
                res = await _SkillPartService.CreateAsync(SkillPartDTO);

            if (res != null && res.Success)
            {
                ToastSuccess(res.Message);
                //return Redirect($"/manager/exam-categories/{CurrentExamCategory.Id}/skill-categories/{CurrentSkillCategory.Id}/skill-parts");
                return await OnGetAsync(exId, skillCatId, id);
            }

            ToastError(res.Message);
            return Page();
        }
    }
}
