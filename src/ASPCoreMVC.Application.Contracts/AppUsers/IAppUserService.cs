using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ASPCoreMVC.AppUsers
{
    public interface IAppUserService : IWrapperCrudAppService<
        AppUserDTO,
        Guid,
        GetAppUserDTO,
        CreateAppUserDTO,
        UpdateAppUserProfileDTO>
    {
        public Task<ResponseWrapper<AppUserProfileDTO>> GetProfileAsync(Guid id);
        public Task<ResponseWrapper<AppUserProfileDTO>> UpdateProfileAsync(Guid id, AppUserProfileDTO profile);
        public Task<ResponseWrapper<AppUserProfileDTO>> GetSelfProfileAsync();
        public Task<ResponseWrapper<AppUserProfileDTO>> UpdateSelfProfileAsync(AppUserProfileDTO profile);
        public Task<ResponseWrapper<bool>> PasswordValidate(string password);
        public Task<ResponseWrapper<string>> GetSelftShortTimeToken();
        public Task<ResponseWrapper<AppUserDTO>> CreateAsync(CreateAppUserDTO input, List<string> roles);
    }
}
