using ASPCoreMVC.TCUEnglish.ExamLogs;
using ASPCoreMVC.Web.Helpers;
using ASPCoreMVC.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.Web.Pages.Dashboard.Partials
{
    [Authorize]
    public class DashboardStudentExamController : Controller
    {
        private readonly IExamLogService _ExamLogService;

        public DashboardStudentExamController(IExamLogService _ExamLogService)
        {
            this._ExamLogService = _ExamLogService;
        }

        [HttpGet("/dasboard/student-exams")]
        public async Task<IActionResult> GetHistoryTable(
            [FromQuery] Guid? studentId,
            [FromQuery] int p = 1)
        {
            var studentExams = await _ExamLogService.GetStudentExams(studentId, new PagedAndSortedResultRequestDto
            {
                SkipCount = (p - 1) * AppTheme.Limit,
                MaxResultCount = AppTheme.Limit
            });

            ViewBag.pz = p;

            ViewBag.Paginationz = PaginateHelper.Generate(
                    "javascript:loadStudentExam('" + studentId + "','{0}');",
                    p, studentExams.TotalCount, AppTheme.Limit);

            return PartialView("~/Pages/Dashboard/Partials/StudentExamLog.Tables.cshtml", studentExams);
        }
        public IActionResult Index()
        {
            return NotFound();
        }
    }
}
