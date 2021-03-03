using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ASPCoreMVC.AppFiles;
using ASPCoreMVC.AppUsers;
using ASPCoreMVC.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPCoreMVC.Web.Pages.Account
{
    [Authorize]
    public class ProfileModel : AppPageModel
    {
        private readonly IAppUserService _AppUserService;
        private readonly IAppFileService _FileAppService;

        [BindProperty]
        public AppUserProfileDTO UserProfile { get; set; }
        [BindProperty]
        public _UploadUserPhotoDTO UserAvatar { get; set; }

        public ProfileModel(
            IAppUserService appUserService,
            IAppFileService fileAppService)
        {
            this._AppUserService = appUserService;
            this._FileAppService = fileAppService;
        }

        public async Task OnGet()
        {
            UserProfile = (await _AppUserService.GetSelfProfileAsync()).Data;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
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
                        if (fRes.Success)
                        {
                            //Cập nhật đường dẫn ảnh vào cho hồ sơ người dùng
                            UserProfile.Picture = $"/resources/{fRes.Data.Name}";
                            ToastSuccess(fRes.Message);
                        }
                        else
                        {
                            ToastError(fRes.Message);
                            return Page();
                        }
                    }
                }
                var res = await _AppUserService.UpdateSelfProfileAsync(UserProfile);
                if (res.Success)
                {
                    UserProfile = res.Data;
                    ToastSuccess(res.Message);
                }
                else
                    ToastError(res.Message);
            }
            else
                ToastError(L["Your input is invalid"]);
            return Page();
        }
    }
}
