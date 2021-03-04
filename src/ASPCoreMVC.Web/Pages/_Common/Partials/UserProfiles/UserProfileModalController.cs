using ASPCoreMVC.AppUsers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace ASPCoreMVC.Web.Pages._Common.Partials.UserProfiles
{
    public class UserProfileModalController : AbpController
    {
        private readonly IAppUserService _AppUserService;
        private readonly IIdentityRoleAppService _IdentityRoleAppService;
        private readonly IIdentityUserAppService _IdentityUserAppService;
        public UserProfileModalController(
            IAppUserService _AppUserService,
            IIdentityRoleAppService _IdentityRoleAppService,
            IIdentityUserAppService _IdentityUserAppService)
        {
            this._AppUserService = _AppUserService;
            this._IdentityRoleAppService = _IdentityRoleAppService;
            this._IdentityUserAppService = _IdentityUserAppService;
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

            // Get all roles of user
            var roleDtos = await _IdentityUserAppService.GetRolesAsync(res.Data.Id);
            var roles = roleDtos.Items.Where(x => x.IsPublic).Select(x => x.Name).JoinAsString(", ");
            if (roleDtos.Items.Count <= 0)
            {
                roles = L["Member"];
            }
            ViewBag.Roles = roles;

            return PartialView("~/Pages/_Common/Partials/UserProfiles/UserProfileModal.cshtml", res.Data);
        }
    }
}
