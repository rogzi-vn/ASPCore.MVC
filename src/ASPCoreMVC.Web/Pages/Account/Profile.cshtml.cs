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
                    {
                        ToastError(L["Trans:PleaseUploadImageOnly"]);
                    }
                    else
                    {
                        // Nếu người dùng đã chọn ảnh đại diện mới
                        using var memoryStream = new MemoryStream();
                        await UserAvatar.File.CopyToAsync(memoryStream);

                        //var AppFile = await _FileAppService.SaveAppFileAsync(
                        //    new SaveAppFileDTO
                        //    {
                        //        Name = UserAvatar.Name,
                        //        Content = memoryStream.ToArray()
                        //    }
                        //);
                        // Cập nhật đường dẫn ảnh vào cho hồ sơ người dùng
                        //UserProfile.Picture = $"/download/{AppFile.Id}_{AppFile.Name}";
                    }
                }
                UserProfile = (await _AppUserService.UpdateSelfProfileAsync(UserProfile)).Data;
                ToastSuccess(L["Trans:UpdateProfileSuccessful"]);
            }
            else
            {
                ToastError(L["Trans:YourInputValueInvalid"]);
            }
            return Page();
        }
    }
}
