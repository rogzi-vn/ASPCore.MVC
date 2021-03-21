using ASPCoreMVC.TCUEnglish.MessGroups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreMVC.Web.Pages._Common.MessageCenter
{
    [Authorize]
    public class MessageCenterController : Controller
    {
        private readonly IMessGroupService _MessGroupService;
        public MessageCenterController(IMessGroupService _MessGroupService)
        {
            this._MessGroupService = _MessGroupService;
        }

        [HttpGet("/message-center/sync")]
        public async Task<IActionResult> SyncAlertCenter()
        {
            var getFilter = new GetMessGroupDTO
            {
                MaxResultCount = 5,
                SkipCount = 0
            };

            var res = await _MessGroupService.GetListAsync(getFilter);
            return PartialView("~/Pages/_Common/MessageCenter/MessageCenter.cshtml", res);
        }
    }
}
