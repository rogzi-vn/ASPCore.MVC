using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPCoreMVC.Web.Pages.Manager.ExamCategories
{
    public class ExamCategoriesIndexModel : AppPageModel
    {
        private readonly IExamCategoryService _ExamCategoryService;

        public List<ExamCategoryBaseDTO> ExamCategories { get; private set; }

        public ExamCategoriesIndexModel(IExamCategoryService examCategoryService)
        {
            _ExamCategoryService = examCategoryService;
        }
        public async Task OnGetAsync()
        {
            var res = await _ExamCategoryService.GetBase();
            if (res.Success)
            {
                ExamCategories = res.Data;
            }
            else
            {
                ToastError(res.Message);
            }
        }
    }
}
