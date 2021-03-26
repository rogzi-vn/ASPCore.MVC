using ASPCoreMVC.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using IdentityServer4;
using ASPCoreMVC.Users;
using ASPCoreMVC.AppUsers;
using ASPCoreMVC._Commons.Services;
using ASPCoreMVC._Commons;

namespace ASPCoreMVC.AppUsers
{
    [Authorize]
    public class AppUserService : WrapperCrudAppService<
        AppUser,
        AppUserDTO,
        Guid,
        GetAppUserDTO,
        CreateAppUserDTO,
        UpdateAppUserProfileDTO>, IAppUserService
    {
        private readonly IdentityUserManager userManager;
        private readonly IdentityServerTools tools;
        public AppUserService(
            IRepository<AppUser, Guid> appUserRepo,
            IdentityUserManager userManager,
            IdentityServerTools tools) : base(appUserRepo)
        {
            this.userManager = userManager;
            this.tools = tools;
            //GetPolicyName = ASPCoreMVCPermissions.UserManager.Default;
            //GetListPolicyName = ASPCoreMVCPermissions.UserManager.Default;
            //CreatePolicyName = ASPCoreMVCPermissions.UserManager.Default;
            //UpdatePolicyName = ASPCoreMVCPermissions.UserManager.Default;
            //DeletePolicyName = ASPCoreMVCPermissions.UserManager.Default;
        }

        [Authorize()]
        [Authorize]
        public async Task<ResponseWrapper<AppUserProfileDTO>> GetProfileAsync(Guid id)
        {
            var res = await Repository.GetAsync(id);
            if (res == null)
                return null;
            return new ResponseWrapper<AppUserProfileDTO>(
                ObjectMapper.Map<AppUser, AppUserProfileDTO>(res),
                "Successful");
        }

        [Authorize]
        public async Task<ResponseWrapper<AppUserProfileDTO>> GetSelfProfileAsync()
        {
            var res = await Repository.GetAsync(CurrentUser.Id.Value);
            if (res == null)
                return null;
            return new ResponseWrapper<AppUserProfileDTO>(
                ObjectMapper.Map<AppUser, AppUserProfileDTO>(res),
                "Successful");
        }

        [Authorize]
        private async Task<ResponseWrapper<AppUserProfileDTO>> _UpdateProfileAsync(Guid id, AppUserProfileDTO profile)
        {
            var res = await Repository.GetAsync(id);
            if (res == null)
                return null;

            var cloneUser = res;
            bool isHaveUpdateImportantInfos = false;

            #region Process email/phone update
            if (cloneUser.Email != profile.Email)
            {
                // If current user email different with new profile email, we will check for update available or not
                if (await Repository.AnyAsync(x => x.Email.Equals(profile.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    // If other user have this email of current profile, notify to user
                    return new ResponseWrapper<AppUserProfileDTO>()
                    .ErrorReponseWrapper(ObjectMapper.Map<AppUser, AppUserProfileDTO>(res), "Email already exists", 403);
                }
                else
                {
                    isHaveUpdateImportantInfos = true;
                    // Set new Email to current user profile
                    cloneUser.SetEmail(profile.Email);

                }
            }

            if (cloneUser.PhoneNumber != profile.PhoneNumber)
            {
                // If current user phone number different with new profile phone number, we will check for update available or not
                if (await Repository.AnyAsync(x => x.PhoneNumber.Equals(profile.PhoneNumber)))
                {
                    // If other user have this phone number of current profile, notify to user
                    return new ResponseWrapper<AppUserProfileDTO>()
                    .ErrorReponseWrapper(ObjectMapper.Map<AppUser, AppUserProfileDTO>(res), "Phone number already exists", 403);
                }
                else
                {
                    isHaveUpdateImportantInfos = true;
                    // Set new phone number to current user profile
                    cloneUser.SetPhoneNumber(profile.PhoneNumber);

                }
            }

            if (isHaveUpdateImportantInfos)
            {
                // Update for current user
                res = await Repository.UpdateAsync(cloneUser);
            }
            #endregion

            #region Process profile update
            var updateAppUserProfileDTO = ObjectMapper.Map<AppUserProfileDTO, UpdateAppUserProfileDTO>(profile);
            // Saving picture path if user update null
            if (updateAppUserProfileDTO.Picture.IsNullOrEmpty())
                updateAppUserProfileDTO.Picture = cloneUser.Picture;

            var updated = await UpdateAsync(id, updateAppUserProfileDTO);
            if (!updated.Success)
                return new ResponseWrapper<AppUserProfileDTO>()
                    .ErrorReponseWrapper(ObjectMapper.Map<AppUser, AppUserProfileDTO>(res), "Update profile faild", 403);
            else
            {
                return new ResponseWrapper<AppUserProfileDTO>(
                    ObjectMapper.Map<AppUserDTO, AppUserProfileDTO>(updated.Data),
                    "Successful");
            }
            #endregion
        }

        //[Authorize(ASPCoreMVCPermissions.UserManager.Default)]
        [Authorize]
        public async Task<ResponseWrapper<AppUserProfileDTO>> UpdateProfileAsync(Guid id, AppUserProfileDTO profile)
        {
            return await _UpdateProfileAsync(id, profile);
        }

        [Authorize]
        public async Task<ResponseWrapper<AppUserProfileDTO>> UpdateSelfProfileAsync(AppUserProfileDTO profile)
        {
            return await _UpdateProfileAsync(CurrentUser.Id.Value, profile);
        }

        [Authorize]
        public override async Task<ResponseWrapper<PagedResultDto<AppUserDTO>>> GetListAsync(GetAppUserDTO input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(AppUser.Name);
            }
            var query = await Repository.GetQueryableAsync();
            if (!input.Filter.IsNullOrWhiteSpace())
            {
                query = query.Where(user =>
                user.DisplayName.Contains(input.Filter) ||
                user.Surname.Contains(input.Filter) ||
                user.Name.Contains(input.Filter) ||
                user.PhoneNumber.Contains(input.Filter) ||
                user.IdentityCardNumber.Contains(input.Filter) ||
                user.UserName.Contains(input.Filter) ||
                user.Email.Contains(input.Filter));
            }
            var users = query
                .OrderBy(input.Sorting)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            return new ResponseWrapper<PagedResultDto<AppUserDTO>>(
                new PagedResultDto<AppUserDTO>(
                query.Count(),
                ObjectMapper.Map<List<AppUser>, List<AppUserDTO>>(users)),
                "Successful");
        }

        [Authorize]
        public override Task<ResponseWrapper<AppUserDTO>> CreateAsync(CreateAppUserDTO input)
        {

            return CreateAsync(input, new List<string>());
        }

        public async Task<ResponseWrapper<AppUserDTO>> CreateAsync(CreateAppUserDTO input, List<string> roles)
        {
            input.Id = Guid.NewGuid();
            var identityUser = ObjectMapper.Map<CreateAppUserDTO, IdentityUser>(input);
            var res = await userManager.CreateAsync(identityUser, input.Password);
            if (res.Succeeded)
            {
                await userManager.SetRolesAsync(identityUser, roles);
                // Khi đã tạo thành công, tiến ành cập nhật lại hồ sơ
                // Lấy user vừa tạo thông qua username
                var user = await Repository.GetAsync(x => x.UserName.Equals(input.UserName, StringComparison.OrdinalIgnoreCase));

                var userProfile = ObjectMapper.Map<CreateAppUserDTO, AppUserProfileDTO>(input);
                var updateAppUserProfileDTO = ObjectMapper.Map<AppUserProfileDTO, UpdateAppUserProfileDTO>(userProfile);
                ObjectMapper.Map(updateAppUserProfileDTO, user);
                user = await Repository.UpdateAsync(user, autoSave: true);
                return new ResponseWrapper<AppUserDTO>(
                    ObjectMapper.Map<AppUser, AppUserDTO>(user),
                    "Successful");
            }
            return null;
        }

        [Authorize]
        public async Task<ResponseWrapper<bool>> PasswordValidate(string password)
        {
            List<string> passwordErrors = new List<string>();

            var validators = userManager.PasswordValidators;

            foreach (var validator in validators)
            {
                var result = await validator.ValidateAsync(userManager, null, password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        passwordErrors.Add(error.Description);
                    }
                    return new ResponseWrapper<bool>(false, "Fail"); ;
                }
            }
            return new ResponseWrapper<bool>(true, "Successful");
        }

        [Authorize]
        public async Task<ResponseWrapper<string>> GetSelftShortTimeToken()
        {
            return new ResponseWrapper<string>(
                "Bearer " + await tools.IssueClientJwtAsync(
                clientId: "ASPCore_Web",
                lifetime: 120,
                audiences: new[] { "ASPCore" }),
                "Successful");
        }


    }
}
