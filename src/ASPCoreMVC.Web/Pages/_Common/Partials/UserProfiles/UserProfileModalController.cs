using ASPCoreMVC.AppUsers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreMVC.Web.Pages._Common.Partials.UserProfiles
{
    public class UserProfileModalController : Controller
    {
        private readonly IAppUserService _AppUserService;
        public UserProfileModalController(IAppUserService _AppUserService)
        {
            this._AppUserService = _AppUserService;
        }

        [HttpGet]
        [Route("/users/{userId:Guid}/profile-modal")]
        public async Task<IActionResult> Index(Guid? userId)
        {
            if (userId == null || userId == Guid.Empty)
                return PartialView(AppTheme.ContentNothing);

            var res = await _AppUserService.GetProfileAsync(userId.Value);
            if (!res.Success || res.Data == null)
                return PartialView(AppTheme.ContentNothing);

            return PartialView("~/Pages/_Common/Partials/UserProfiles/UserProfileModal.cshtml", res.Data);
        }
    }
}
