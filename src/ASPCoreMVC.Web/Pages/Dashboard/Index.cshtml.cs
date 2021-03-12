using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPCoreMVC.Web.Pages.Dashboard
{
    [Authorize]
    public class DashboardIndexModel : AppPageModel
    {
        private readonly IExamCategoryService _ExamCategoryService;

        public DashboardIndexModel(IExamCategoryService _ExamCategoryService)
        {
            this._ExamCategoryService = _ExamCategoryService;
        }

        public List<ExamCategoryBaseDTO> ExamCats { get; set; } = new List<ExamCategoryBaseDTO>();

        public ExamCategoryBaseDTO CurrentExamCategory { get; set; }

        [BindProperty]
        public Guid? ExamCategoryId { get; set; }

        public async Task OnGetAsync()
        {
            await PreProcess();
        }

        public async Task OnPostAsync()
        {
            await PreProcess();
        }

        private async Task PreProcess()
        {
            // Lấy danh sách các loại kỳ thi hiện đang có
            var res = await _ExamCategoryService.GetBase();
            if (res.Success && res.Data != null)
                ExamCats = res.Data;

            // Nếu không được truyền giá trị là hiển thị thống kê cho kỳ thi nào thì tiến hành hiển thị cho record kỳ thi đầu tiên
            if (ExamCategoryId == null || ExamCategoryId == Guid.Empty)
                ExamCategoryId = ExamCats.FirstOrDefault()?.Id ?? Guid.Empty;

            CurrentExamCategory = ExamCats.First(x => x.Id == ExamCategoryId);
        }
    }
}
