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
    public class DashboardHistoryController : Controller
    {
        private readonly IExamLogService _ExamLogService;

        public DashboardHistoryController(IExamLogService _ExamLogService)
        {
            this._ExamLogService = _ExamLogService;
        }

        [HttpGet("/dasboard/histories")]
        public async Task<IActionResult> GetHistoryTable(
            [FromQuery] Guid? desId,
            [FromQuery] int p = 1)
        {
            var stats = await _ExamLogService.GetExamHistoryStats(desId);
            var histories = await _ExamLogService.GetExamHistories(desId, new PagedAndSortedResultRequestDto
            {
                SkipCount = (p - 1) * AppTheme.Limit,
                MaxResultCount = AppTheme.Limit
            });

            ViewBag.p = p;

            ViewBag.Pagination = PaginateHelper.Generate(
                    "javascript:loadHistories('" + desId + "','{0}');",
                    p, histories.TotalCount, AppTheme.Limit);

            return PartialView("~/Pages/Dashboard/Partials/History.Tables.cshtml", new DashboardHistoryViewModel
            {
                Stats = stats,
                Histories = histories
            });
        }

        public IActionResult Index()
        {
            return NotFound();
        }
    }
}
