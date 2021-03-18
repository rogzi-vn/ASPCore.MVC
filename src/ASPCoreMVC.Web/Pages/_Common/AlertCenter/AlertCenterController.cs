using ASPCoreMVC.TCUEnglish.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.Web.Pages._Common.AlertCenter
{
    [Authorize]
    public class AlertCenterController : Controller
    {
        private readonly INotificationService _NotificationService;
        public AlertCenterController(INotificationService _NotificationService)
        {
            this._NotificationService = _NotificationService;
        }

        [HttpGet("/alert-center/sync")]
        public async Task<IActionResult> SyncAlertCenter()
        {
            var lastNewNotifications = await _NotificationService.GetListAsync(new PagedAndSortedResultRequestDto
            {
                MaxResultCount = 3,
                SkipCount = 0
            });
            return PartialView("~/Pages/_Common/AlertCenter/AlertCenter.cshtml", lastNewNotifications);
        }
    }
}
