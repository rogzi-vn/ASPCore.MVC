using System;
using System.Threading.Tasks;
using ASPCoreMVC._Commons;
using ASPCoreMVC.Permissions;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPCoreMVC.Web.Pages.Manager.ExamCategories.Partials
{
    [Authorize(ASPCoreMVCPermissions.ExamManager.Default)]
    public class ExamCategoryCreateUpdateModel : AppPageModel
    {
        private readonly IExamCategoryService _ExamCategoryService;

        [BindProperty]
        public CreateUpdateExamCategoryDTO ExamCategoryDTO { get; set; }

        public ExamCategoryCreateUpdateModel(IExamCategoryService examCategoryService)
        {
            _ExamCategoryService = examCategoryService;
        }
        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            // Create case
            if (id == null || id == Guid.Empty)
            {
                ExamCategoryDTO = new CreateUpdateExamCategoryDTO();
                return Page();
            }

            // Update case
            var examCat = await _ExamCategoryService.GetForUpdate(id.Value);
            if (examCat.Success)
            {
                ExamCategoryDTO = examCat.Data;
                return Page();
            }

            ToastError(examCat.Message);
            return Redirect($"/manager/exam-categories");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ToastError(L["Your input is invalid"]);
                return Page();
            }

            ResponseWrapper<ExamCategoryDTO> res;
            if (ExamCategoryDTO.Id != Guid.Empty)
                res = await _ExamCategoryService.UpdateAsync(ExamCategoryDTO.Id, ExamCategoryDTO);
            else
                res = await _ExamCategoryService.CreateAsync(ExamCategoryDTO);

            if (res != null)
            {
                ToastSuccess(res.Message);
                return Redirect("/manager/exam-categories");
            }
            else
            {
                ToastError(res.Message);
                return Page();
            }
        }
    }
}
