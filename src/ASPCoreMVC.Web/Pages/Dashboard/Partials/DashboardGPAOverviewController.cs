using ASPCoreMVC.Helpers;
using ASPCoreMVC.TCUEnglish.ScoreLogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreMVC.Web.Pages.Dashboard.Partials
{
    [Authorize]
    public class DashboardGPAOverviewController : Controller
    {
        private readonly IScoreLogService _ScoreLogService;

        public DashboardGPAOverviewController(IScoreLogService _ScoreLogService)
        {
            this._ScoreLogService = _ScoreLogService;
        }

        [HttpGet("/analytics/gpa/week/{exCatId:Guid}")]
        public async Task<IActionResult> WeekGPA(Guid exCatId)
        {
            var res = await _ScoreLogService.GetGpaUptoNow(exCatId, DateTimeHelper.GetMonday());
            return Json(res);
        }

        [HttpGet("/analytics/gpa/month/{exCatId:Guid}")]
        public async Task<IActionResult> MonthGPA(Guid exCatId)
        {
            var res = await _ScoreLogService.GetGpaUptoNow(exCatId, DateTimeHelper.GetFirstDayOfMonth());
            return Json(res);
        }

        [HttpGet("/analytics/gpa/year/{exCatId:Guid}")]
        public async Task<IActionResult> YearGPA(Guid exCatId)
        {
            var res = await _ScoreLogService.GetGpaUptoNow(exCatId, DateTimeHelper.GetFirstDayOfYear());
            return Json(res);
        }
    }
}
