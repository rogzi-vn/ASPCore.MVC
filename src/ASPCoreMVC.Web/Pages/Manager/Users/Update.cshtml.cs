using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCoreMVC.AppUsers;
using ASPCoreMVC.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.Web.Pages.Manager.Users
{
    public class UpdateUserModel : AppPageModel
    {
        private readonly IAppUserService _AppUserService;

        private readonly IRepository<AppUser, Guid> _IAppUserRepository;

        public UpdateUserModel(
            IAppUserService _AppUserService,
            IRepository<AppUser, Guid> _IAppUserRepository)
        {
            this._AppUserService = _AppUserService;
            this._IAppUserRepository = _IAppUserRepository;
        }

        [BindProperty]
        public AppUserProfileDTO UserAccount { get; set; }
        [BindProperty]
        public string UserNewPassword { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid? userId)
        {
            if (userId == null || userId == Guid.Empty)
            {
                ToastError(L["User not found"]);
                return Redirect("/manager/users");
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
