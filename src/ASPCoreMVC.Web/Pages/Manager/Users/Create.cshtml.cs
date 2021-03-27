using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCoreMVC.AppUsers;
using ASPCoreMVC.Permissions;
using ASPCoreMVC.Users;
using ASPCoreMVC.Web.Models;
using ASPCoreMVC.Web.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Settings;

namespace ASPCoreMVC.Web.Pages.Manager.Users
{
    [Authorize(ASPCoreMVCPermissions.UserManager.Default)]
    public class CreateUserModel : AppPageModel
    {
        private readonly ISettingProvider _SettingProvider;
        private readonly IAppUserService _AppUserService;

        private readonly IIdentityRoleAppService _IdentityRoleAppService;
        private readonly IdentityUserManager _IdentityUserManager;

        private readonly IRepository<AppUser, Guid> _IAppUserRepository;

        public CreateUserModel(
            ISettingProvider _SettingProvider,
            IAppUserService _AppUserService,
            IIdentityRoleAppService _IdentityRoleAppService,
            IdentityUserManager _IdentityUserManager,
            IRepository<AppUser, Guid> _IAppUserRepository)
        {
            this._SettingProvider = _SettingProvider;
            this._AppUserService = _AppUserService;
            this._IAppUserRepository = _IAppUserRepository;
            this._IdentityRoleAppService = _IdentityRoleAppService;
            this._IdentityUserManager = _IdentityUserManager;
        }

        [BindProperty]
        public CreateAppUserDTO UserAccount { get; set; }

        [BindProperty]
        public List<PairSelector> CurrentUserRoles { get; set; }

        public async Task OnGetAsync()
        {
            CurrentUserRoles = new List<PairSelector>();
            var roles = (await _IdentityRoleAppService.GetAllListAsync())
                .Items.Where(x => x.IsPublic).Select(x => x.Name).ToList();
            foreach (var role in roles)
            {
                CurrentUserRoles.Add(new PairSelector { Key = role, Value = false });
            }
            UserAccount = new CreateAppUserDTO
            {
                Password = await _SettingProvider.GetOrNullAsync(PageSettingProvider.USER_CREATOR_DEFAULT_PASSWORD_FOR_NEW_USER) ?? ""
            };
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!UserAccount.Email.IsNullOrEmpty() && await _IAppUserRepository.AnyAsync(x => x.Email.Equals(UserAccount.Email, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("UserAccount.Email", L["The email address has been used"]);
            }
            if (!UserAccount.UserName.IsNullOrEmpty() && await _IAppUserRepository.AnyAsync(x => x.UserName.Equals(UserAccount.UserName, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("UserAccount.UserName", L["The username has been used"]);
            }
            if (!UserAccount.PhoneNumber.IsNullOrEmpty() && await _IAppUserRepository.AnyAsync(x => x.PhoneNumber.Equals(UserAccount.PhoneNumber, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("UserAccount.PhoneNumber", L["The phone number has been used"]);
            }

            if (!ModelState.IsValid)
            {
                ToastError(L["Your input is invalid"]);
                return Page();
            }


            var res = await _AppUserService.CreateWithRoleAsync(UserAccount,
                CurrentUserRoles.Where(x => x.Value).Select(x => x.Key).ToList());
            if (res.Success)
            {
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
