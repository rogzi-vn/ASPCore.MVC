using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCoreMVC.AppUsers;
using ASPCoreMVC.Permissions;
using ASPCoreMVC.Users;
using ASPCoreMVC.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace ASPCoreMVC.Web.Pages.Manager.Users
{
    [Authorize(ASPCoreMVCPermissions.UserManager.Default)]
    public class UpdateUserModel : AppPageModel
    {
        private readonly IAppUserService _AppUserService;

        private readonly IIdentityRoleAppService _IdentityRoleAppService;
        private readonly IdentityUserManager _IdentityUserManager;

        private readonly IRepository<AppUser, Guid> _IAppUserRepository;

        public UpdateUserModel(
            IAppUserService _AppUserService,
            IIdentityRoleAppService _IdentityRoleAppService,
            IdentityUserManager _IdentityUserManager,
            IRepository<AppUser, Guid> _IAppUserRepository)
        {
            this._IAppUserRepository = _IAppUserRepository;
            this._IdentityRoleAppService = _IdentityRoleAppService;
            this._AppUserService = _AppUserService;
            this._IdentityUserManager = _IdentityUserManager;
        }

        [BindProperty]
        public AppUserProfileDTO UserAccount { get; set; }
        [BindProperty]
        public string UserNewPassword { get; set; }

        [BindProperty]
        public List<PairSelector> CurrentUserRoles { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? userId)
        {
            if (userId == null || userId == Guid.Empty)
            {
                ToastError(L["User not found"]);
                return Redirect("/manager/users");
            }

            // Get current user
            var currentUser = await _IdentityUserManager.GetByIdAsync(userId.Value);
            var currentUserOwnRoles = await _IdentityUserManager.GetRolesAsync(currentUser);

            CurrentUserRoles = new List<PairSelector>();
            var roles = (await _IdentityRoleAppService.GetAllListAsync())
                .Items.Where(x => x.IsPublic).Select(x => x.Name).ToList();
            foreach (var role in roles)
            {
                CurrentUserRoles.Add(new PairSelector
                {
                    Key = role,
                    Value = currentUserOwnRoles.Any(x => x == role)
                });
            }

            var res = await _AppUserService.GetProfileAsync(userId.Value);
            if (res.Success && res.Data != null)
            {
                UserAccount = res.Data;
                return Page();
            }
            else
            {
                ToastError(L["User not found"]);
                return Redirect("/manager/users");
            }

        }
        public async Task<IActionResult> OnPostAsync(Guid userId)
        {
            if (!UserAccount.Email.IsNullOrEmpty() &&
                await _IAppUserRepository
                .AnyAsync(x => x.Email.Equals(UserAccount.Email, StringComparison.OrdinalIgnoreCase) &&
                x.Id != userId))
            {
                ModelState.AddModelError("UserAccount.Email", L["The email address has been used"]);
            }
            if (!UserAccount.PhoneNumber.IsNullOrEmpty() &&
                await _IAppUserRepository
                .AnyAsync(x => x.PhoneNumber.Equals(UserAccount.PhoneNumber, StringComparison.OrdinalIgnoreCase) &&
                x.Id != userId))
            {
                ModelState.AddModelError("UserAccount.PhoneNumber", L["The phone number has been used"]);
            }

            if (!ModelState.IsValid)
            {
                ToastError(L["Your input is invalid"]);
                return Page();
            }


            var res = await _AppUserService.UpdateProfileAsync(userId, UserAccount);
            if (res.Success)
            {
                // Get created user
                var createdUser = await _IdentityUserManager.GetByIdAsync(res.Data.Id);
                // Add roles
                await _IdentityUserManager.SetRolesAsync(createdUser, CurrentUserRoles.Where(x => x.Value).Select(x => x.Key));
                await _IdentityUserManager.RemoveFromRolesAsync(createdUser, CurrentUserRoles.Where(x => !x.Value).Select(x => x.Key));
                ToastSuccess(res.Message);
                return Redirect("/manager/users");
            }
            else
            {
                ToastError(res.Message);
                return Page();
            }
        }
    }
}
