using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using ASPCoreMVC.TCUEnglish.ExamCatInstructors;
using ASPCoreMVC.TCUEnglish.ExamLogs;
using ASPCoreMVC.TCUEnglish.ScoreLogs;
using ASPCoreMVC.TCUEnglish.SkillCategories;
using ASPCoreMVC.TCUEnglish.SkillParts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.Web.Pages.Dashboard
{
    [Authorize]
    public class DashboardIndexModel : AppPageModel
    {
        private readonly IExamCategoryService _ExamCategoryService;
        private readonly IExamLogService _ExamLogService;
        private readonly IScoreLogService _ScoreLogService;

        private readonly IReadOnlyRepository<ExamCatInstructor, Guid> _ExamCatInstructorRepository;

        public DashboardIndexModel(
            IExamCategoryService _ExamCategoryService,
            IExamLogService _ExamLogService,
            IScoreLogService _ScoreLogService,
            IReadOnlyRepository<ExamCatInstructor, Guid> _ExamCatInstructorRepository)
        {
            this._ExamCategoryService = _ExamCategoryService;
            this._ExamLogService = _ExamLogService;
            this._ScoreLogService = _ScoreLogService;
            this._ExamCatInstructorRepository = _ExamCatInstructorRepository;
        }

        public List<ExamCategoryBaseDTO> ExamCats { get; set; } = new List<ExamCategoryBaseDTO>();

        public ExamCategoryBaseDTO CurrentExamCategory { get; set; }

        [BindProperty]
        public Guid? ExamCategoryId { get; set; }

        public int CompletedTests { get; set; }
        public int PassedTests { get; set; }
        public int FailedTests { get; set; }
        public float UserGPA { get; set; }
        public float UserGPARate { get; set; }

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

            // Lấy các thông số thống kê cơ bản
            CompletedTests = await _ExamLogService.GetCompletedTest(ExamCategoryId.Value);
            PassedTests = await _ExamLogService.GetPassedTest(ExamCategoryId.Value);
            FailedTests = await _ExamLogService.GetFaildTest(ExamCategoryId.Value);
            UserGPA = await _ScoreLogService.GetExamCategoryGPA(ExamCategoryId.Value);

            // Lấy điểm cân bằng kỹ năng
            var balanceSkills = await _ScoreLogService.GetSkillCategoryGPAs(ExamCategoryId.Value);
            ViewData["BalanceSkills"] = balanceSkills;
            ViewData["BalanceSkillsJson"] = JsonConvert.SerializeObject(balanceSkills);

            // Lấy điểm tối đa của phần thi
            var examCategoryMaxScores = _ExamCategoryService.GetMaxScores(ExamCategoryId.Value);
            UserGPARate = UserGPA / examCategoryMaxScores;

            CurrentExamCategory = ExamCats.First(x => x.Id == ExamCategoryId);

            // Lấy danh sách skill cat
            ViewData["AllSkillCats"] = await _ExamCategoryService.GetFullChild(ExamCategoryId.Value);
            ViewData["CurrentExamCatId"] = ExamCategoryId.Value;
            ViewData["CurrentExamCatName"] = CurrentExamCategory.Name;

            var isInstructor = false;
            // Kiểm tra xem đây có phải là giáo viên hướng dẫn không
            if (await _ExamCatInstructorRepository.AnyAsync(x => x.UserId == CurrentUser.Id && x.ExamCategoryId == ExamCategoryId.Value))
            {
                isInstructor = true;
                ViewData["IsInstructor"] = isInstructor;

                // Lấy danh sách học viên
                ViewData["Students"] = await _ExamLogService.GetExamLogStudents(ExamCategoryId.Value);
            }

        }
    }
}
