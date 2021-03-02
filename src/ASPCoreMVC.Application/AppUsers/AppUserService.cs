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
            GetPolicyName = ASPCoreMVCPermissions.UserProfiles.Default;
            GetListPolicyName = ASPCoreMVCPermissions.UserProfiles.Default;
            CreatePolicyName = ASPCoreMVCPermissions.UserProfiles.Create;
            UpdatePolicyName = ASPCoreMVCPermissions.UserProfiles.Edit;
            DeletePolicyName = ASPCoreMVCPermissions.UserProfiles.Delete;
        }

        [Authorize(ASPCoreMVCPermissions.UserProfiles.Default)]
        public async Task<ResponseWrapper<AppUserProfileDTO>> GetProfileAsync(Guid id)
        {
            var res = await Repository.GetAsync(id);
            if (res == null)
                return null;
            return new ResponseWrapper<AppUserProfileDTO>(
                ObjectMapper.Map<AppUser, AppUserProfileDTO>(res),
                "Successful");
        }

        public async Task<ResponseWrapper<AppUserProfileDTO>> GetSelfProfileAsync()
        {
            var res = await Repository.GetAsync(CurrentUser.Id.Value);
            if (res == null)
                return null;
            return new ResponseWrapper<AppUserProfileDTO>(
                ObjectMapper.Map<AppUser, AppUserProfileDTO>(res),
                "Successful");
        }

        [Authorize(ASPCoreMVCPermissions.UserProfiles.Edit)]
        public async Task<ResponseWrapper<AppUserProfileDTO>> UpdateProfileAsync(Guid id, AppUserProfileDTO profile)
        {
            var res = await Repository.GetAsync(id);
            if (res == null)
                return null;
            var updateAppUserProfileDTO = ObjectMapper.Map<AppUserProfileDTO, UpdateAppUserProfileDTO>(profile);
            ObjectMapper.Map(updateAppUserProfileDTO, res);
            res = await Repository.UpdateAsync(res);
            return new ResponseWrapper<AppUserProfileDTO>(
                ObjectMapper.Map<AppUser, AppUserProfileDTO>(res),
                "Successful");
        }

        public async Task<ResponseWrapper<AppUserProfileDTO>> UpdateSelfProfileAsync(AppUserProfileDTO profile)
        {
            var res = await Repository.GetAsync(CurrentUser.Id.Value);
            if (res == null)
                return null;
            var updateAppUserProfileDTO = ObjectMapper.Map<AppUserProfileDTO, UpdateAppUserProfileDTO>(profile);
            ObjectMapper.Map(updateAppUserProfileDTO, res);
            res = await Repository.UpdateAsync(res);
            return new ResponseWrapper<AppUserProfileDTO>(
                ObjectMapper.Map<AppUser, AppUserProfileDTO>(res),
                "Successful");
        }

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

        public override async Task<ResponseWrapper<AppUserDTO>> CreateAsync(CreateAppUserDTO input)
        {
            input.Id = Guid.NewGuid();
            var identityUser = ObjectMapper.Map<CreateAppUserDTO, IdentityUser>(input);
            var res = await userManager.CreateAsync(identityUser, input.Password);
            if (res.Succeeded)
            {
                // Khi đã tạo thành công, tiến ành cập nhật lại hồ sơ
                // Lấy user vừa tạo thông qua username
                var user = await Repository.GetAsync(x => x.UserName.Equals(input.UserName, StringComparison.OrdinalIgnoreCase));

                var userProfile = ObjectMapper.Map<CreateAppUserDTO, AppUserProfileDTO>(input);
                var updateAppUserProfileDTO = ObjectMapper.Map<AppUserProfileDTO, UpdateAppUserProfileDTO>(userProfile);
                ObjectMapper.Map(updateAppUserProfileDTO, user);
                user = await Repository.UpdateAsync(user);
                return new ResponseWrapper<AppUserDTO>(
                    ObjectMapper.Map<AppUser, AppUserDTO>(user),
                    "Successful");
            }
            return null;
        }

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
