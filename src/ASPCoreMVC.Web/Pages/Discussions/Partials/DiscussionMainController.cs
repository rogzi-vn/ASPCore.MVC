using ASPCoreMVC.TCUEnglish.MessGroups;
using ASPCoreMVC.TCUEnglish.UserMessages;
using ASPCoreMVC.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreMVC.Web.Pages.Discussions.Partials
{
    [Authorize]
    public class DiscussionMainController : Controller
    {
        private readonly IMessGroupService _MessGroupService;
        private readonly IUserMessageService _UserMessageService;

        public DiscussionMainController(
            IMessGroupService _MessGroupService,
            IUserMessageService _UserMessageService)
        {
            this._MessGroupService = _MessGroupService;
            this._UserMessageService = _UserMessageService;
        }

        [HttpGet("/latest-discussion")]
        public async Task<IActionResult> LatestDiscussion(
            [FromQuery] int p = 1,
            [FromQuery] string filter = "")
        {
            var getFilter = new GetMessGroupDTO
            {
                Filter = filter,
                MaxResultCount = 5,
                SkipCount = (p - 1) * 5
            };

            var res = await _MessGroupService.GetListAsync(getFilter);

            string listRes = string.Format("Showing {0} to {1} of {2} discussion",
                    res.TotalCount > 0 ? getFilter.SkipCount + 1 : 0, getFilter.SkipCount + res.Items.Count, res.TotalCount);
            if (!filter.IsNullOrEmpty())
            {
                listRes += string.Format(" for \"{0}\"", getFilter.Filter);
            }
            ViewBag.ListState = listRes;

            ViewBag.Filter = filter;
            ViewBag.Pagination = PaginateHelper.Generate(
                "javascript:syncVt('{0}', '" + filter + "');",
                p, res.TotalCount, AppTheme.Limit);

            return PartialView("~/Pages/Discussions/Partials/LatestDiscussion.cshtml", res);
        }

        [HttpGet("/load-previous-messages")]
        public async Task<IActionResult> LoadPreviousMessage([FromQuery] Guid messGroupId,
                        [FromQuery] Guid? oldestUserMessageId)
        {
            var res = await _UserMessageService.GetPreviousMessages(messGroupId, 10, oldestUserMessageId);
            return PartialView("~/Pages/Discussions/Partials/MessageBox.cshtml", res);
        }

        [HttpPost("/render-message/{msgId:Guid?}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> RenderMessage([FromRoute] Guid? msgId, [FromBody] UserMessageDTO msg)
        {
            if (msgId != null && msgId != Guid.Empty)
            {
                msg = await _UserMessageService.GetAsync(msgId.Value);
            }
            return PartialView("~/Pages/Discussions/Partials/MessageRender.cshtml", msg);
        }
    }
}
