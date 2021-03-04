using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ASPCoreMVC.AppFiles;
using ASPCoreMVC.AppUsers;
using ASPCoreMVC.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace ASPCoreMVC.Web.Pages.Account
{
    [Authorize]
    public class ProfileModel : AppPageModel
    {
        private readonly IAppUserService _AppUserService;
        private readonly IAppFileService _FileAppService;
        private readonly IdentityUserManager _IdentityUserManager;

        [BindProperty]
        public AppUserProfileDTO UserProfile { get; set; }
        [BindProperty]
        public _UploadUserPhotoDTO UserAvatar { get; set; }
        [BindProperty]
        public AppUserPasswordDTO AppUserPassword { get; set; }

        public ProfileModel(
            IAppUserService appUserService,
            IAppFileService fileAppService,
            IdentityUserManager _IdentityUserManager)
        {
            this._AppUserService = appUserService;
            this._FileAppService = fileAppService;
            this._IdentityUserManager = _IdentityUserManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var res = await _AppUserService.GetSelfProfileAsync();
            if (!res.Success)
            {
                ToastError(L["Can not get your profile"]);
                return Redirect("/");
            }
            UserProfile = res.Data;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            #region Validate process
            bool isUpdatePasswordSignal = !AppUserPassword.CurrentPassword.IsNullOrEmpty() ||
                !AppUserPassword.NewPassword.IsNullOrEmpty() ||
                !AppUserPassword.ConfirmNewPassword.IsNullOrEmpty();
            // Check change password validate
            if (isUpdatePasswordSignal)
            {
                // If User input any field in password change, check full fill of input
                if (AppUserPassword.CurrentPassword.IsNullOrEmpty())
                    ModelState.AddModelError("AppUserPassword.CurrentPassword", L["Please input your current password"]);
                if (AppUserPassword.NewPassword.IsNullOrEmpty())
                    ModelState.AddModelError("AppUserPassword.NewPassword", L["Please input your new password"]);
                if (AppUserPassword.ConfirmNewPassword.IsNullOrEmpty())
                    ModelState.AddModelError("AppUserPassword.ConfirmNewPassword", L["Please input confirm of your new password"]);
            }

            // Check for model state valid
            if (!ModelState.IsValid)
            {
                ToastError(L["Your input is invalid"]);
                return Page();
            }
            #endregion

            #region Check and upate user picture
            if (UserAvatar.File != null && UserAvatar.File.Length > 0 &&
                    !UserAvatar.Name.IsNullOrEmpty())
            {
                if (!UserAvatar.File.IsImage())
                    ToastError(L["Please upload image only"]);
                else
                {
                    // Nếu người dùng đã chọn ảnh đại diện mới
                    using var memoryStream = new MemoryStream();
                    await UserAvatar.File.CopyToAsync(memoryStream);

                    var fRes = await _FileAppService.PostUserAvatarUploadAsync(
                        new RawAppFileDTO
                        {
                            Name = UserAvatar.Name,
                            Content = memoryStream.ToArray()
                        }
                    );
                    if (!fRes.Success)
                    {
                        ToastError(fRes.Message);
                        return Page();
                    }

                    //Cập nhật đường dẫn ảnh vào cho hồ sơ người dùng
                    UserProfile.Picture = $"/resources/{fRes.Data.Name}";
                    ToastSuccess(fRes.Message);

                }
            }
            #endregion

            #region Update current user profile
            var previousProfileRes = await _AppUserService.GetSelfProfileAsync();
            if (!previousProfileRes.Success)
            {
                ToastError(L["Can not get your profile"]);
                return Redirect("/");
            }
            var previousProfile = previousProfileRes.Data;
            if (previousProfile.CheckChange(UserProfile))
            {
                // if profile have any change, process update it
                var res = await _AppUserService.UpdateSelfProfileAsync(UserProfile);
                if (res.Success)
                {
                    UserProfile = res.Data;
                    ToastSuccess(res.Message);
                }
                else
                    ToastError(res.Message);
            }
            #endregion

            #region Update current user password
            if (isUpdatePasswordSignal)
            {
                // If have signal of password change, process it
                var currentIdentityUser = await _IdentityUserManager.GetByIdAsync(CurrentUser.Id.Value);
                if (!await _IdentityUserManager.CheckPasswordAsync(currentIdentityUser, AppUserPassword.CurrentPassword))
                    // If current input of user password not correct, ignore update
                    ToastError(L["Your current password is incorrect"]);
                else if (AppUserPassword.NewPassword != AppUserPassword.ConfirmNewPassword)
                    // If new password different from self confirm, Ignore update
                    ToastError(L["Confrim password is incorrect"]);
                else
                {
                    // Process update password
                    var result = await _IdentityUserManager.ChangePasswordAsync(currentIdentityUser, AppUserPassword.CurrentPassword, AppUserPassword.NewPassword);
                    if (result.Succeeded)
                        ToastSuccess(L["Changed password successful"]);
                    else if (result.Errors.Count() > 0)
                        ToastError(L[result.Errors.FirstOrDefault()?.Description ?? ""]);
                    else
                        ToastError(L["Change password faild"]);
                }
            }
            #endregion

            return Page();
        }
    }
}
