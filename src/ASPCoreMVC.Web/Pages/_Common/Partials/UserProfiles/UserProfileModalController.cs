using ASPCoreMVC.AppUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace ASPCoreMVC.Web.Pages._Common.Partials.UserProfiles
{
    [Authorize]
    public class UserProfileModalController : AbpController
    {
        private readonly IAppUserService _AppUserService;
        private readonly IdentityUserManager UserManager;
        public UserProfileModalController(
            IAppUserService _AppUserService,
            IdentityUserManager UserManager)
        {
            this._AppUserService = _AppUserService;
            this.UserManager = UserManager;
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

            // Get current User
            var currentUser = await UserManager.GetByIdAsync(userId.Value);

            // Get all roles of user
            var roles = await UserManager.GetRolesAsync(currentUser);

            //var roleDtos = await _IdentityUserAppService.GetRolesAsync(res.Data.Id);
            //var roles = roleDtos.Items.Where(x => x.IsPublic).Select(x => x.Name).JoinAsString(", ");

            var displayRoles = roles.JoinAsString(", ");
            if (displayRoles.Length <= 0)
            {
                displayRoles = L["Member"];
            }

            ViewBag.Roles = displayRoles;

            return PartialView("~/Pages/_Common/Partials/UserProfiles/UserProfileModal.cshtml", res.Data);
        }
    }
}
