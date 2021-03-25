using ASPCoreMVC._Commons;
using ASPCoreMVC.Permissions;
using ASPCoreMVC.TCUEnglish.GrammarCategories;
using ASPCoreMVC.TCUEnglish.Grammars;
using ASPCoreMVC.TCUEnglish.UserNotes;
using ASPCoreMVC.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace ASPCoreMVC.Web.Pages.Manager.Roles.Partials
{
    [Authorize]
    [Route("/manager/roles")]
    public class PartialsController : AbpController
    {
        private readonly IIdentityRoleAppService _IdentityRoleAppService;

        private readonly IPermissionManager _PermissionManager;

        private static string CreateView = "~/Pages/Manager/Roles/Partials/Create.cshtml";
        private static string UpdateView = "~/Pages/Manager/Roles/Partials/Update.cshtml";
        private static string TableView = "~/Pages/Manager/Roles/Partials/Table.cshtml";

        public PartialsController(
            IIdentityRoleAppService _IdentityRoleAppService,
            IPermissionManager _PermissionManager)
        {
            this._IdentityRoleAppService = _IdentityRoleAppService;
            this._PermissionManager = _PermissionManager;
        }

        [Route("display")]
        [HttpGet]
        public async Task<IActionResult> GetAsync(
            [FromQuery] int? p = 1,
            [FromQuery] string filter = "")
        {
            if (p == null || p <= 0) p = 1;
            ViewBag.p = p.Value;

            var serchInp = new GetIdentityRolesInput
            {
                Filter = filter ?? "",
                MaxResultCount = AppTheme.Limit,
                SkipCount = (p.Value - 1) * AppTheme.Limit
            };
            var res = await _IdentityRoleAppService.GetListAsync(serchInp);

            string listRes = string.Format("Showing {0} to {1} of {2} entries",
                res.TotalCount > 0 ? serchInp.SkipCount + 1 : 0, serchInp.SkipCount + res.Items.Count, res.TotalCount);
            if (!filter.IsNullOrEmpty())
            {
                listRes += string.Format(" for \"{0}\"", serchInp.Filter);
            }
            ViewBag.ListState = listRes;

            ViewBag.Filter = filter;
            ViewBag.Pagination = PaginateHelper.Generate(
                "javascript:syncVt('{0}', '" + filter + "');",
                p.Value, res.TotalCount, AppTheme.Limit);
            return PartialView(TableView, res);
        }

        [HttpGet]
        [Route("create")]
        public IActionResult GetCreateAsync()
        {
            return PartialView(CreateView);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> PostCreateAsync(
            [FromBody] IdentityRoleCreateDto role,
            [FromQuery] string permissions)
        {
            var res = await _IdentityRoleAppService
                .CreateAsync(role);
            if (res != null)
            {
                var ignorePermission = new List<string> {
                    ASPCoreMVCPermissions.GroupName,
                    "AbpIdentity",
                    "FeatureManagement",
                    "AbpTenantManagement"
                    };
                foreach (var permission in permissions.Split(","))
                {
                    if (!ignorePermission.Any(x => x.Equals(permission, StringComparison.OrdinalIgnoreCase)))
                    {
                        await _PermissionManager.SetForRoleAsync(res.Name, permission, true);
                    }

                }
                return Json(new ResponseWrapper<IdentityRoleDto>().SuccessReponseWrapper(res, "Create new role successful"));
            }
            else
                return Json(new ResponseWrapper<IdentityRoleDto>().ErrorReponseWrapper(default, "Create new role successful", 400));
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteUpdateAsync(Guid id)
        {
            var res = await _IdentityRoleAppService.GetAsync(id);
            if (res != null)
            {
                // Delete all permission
                await _PermissionManager.DeleteAsync("R", res.Name);
                // Delete role name
                await _IdentityRoleAppService.DeleteAsync(id);
                return Json(new ResponseWrapper<IdentityRoleDto>().SuccessReponseWrapper(res, $"Delete role {res.Name} successful"));
            }
            return Json(new ResponseWrapper<IdentityRoleDto>().ErrorReponseWrapper(null, "Faild", 500));
        }

        [HttpGet]
        [Route("update/{id}")]
        public async Task<IActionResult> GetUpdateAsync(Guid id)
        {
            var res = await _IdentityRoleAppService.GetAsync(id);
            var permissions = await _PermissionManager.GetAllForRoleAsync(res.Name);
            ViewBag.Permissions = permissions.Where(x => x.IsGranted).Select(x => x.Name).ToList().JoinAsString(","); ;
            return PartialView(UpdateView, ObjectMapper.Map<IdentityRoleDto, IdentityRoleUpdateDto>(res));
        }
        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> PutUpdateAsync(
            [FromRoute] Guid id,
            [FromBody] IdentityRoleUpdateDto role,
            [FromQuery] string permissions)
        {
            var ignorePermission = new List<string> {
            ASPCoreMVCPermissions.GroupName,
            "AbpIdentity",
            "FeatureManagement",
            "AbpTenantManagement"
            };
            var previousRoleName = await _IdentityRoleAppService.GetAsync(id);
            if (previousRoleName.Name.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                return Json(new ResponseWrapper<IdentityRoleDto>().SuccessReponseWrapper(default, "Update new role successful"));
            }
            var res = await _IdentityRoleAppService
                .UpdateAsync(id, role);
            if (res != null)
            {
                // Delete old permission
                await _PermissionManager.DeleteAsync("R", previousRoleName.Name);
                // Add new permisison
                foreach (var permission in permissions.Split(","))
                {
                    if (!ignorePermission.Any(x => x.Equals(permission, StringComparison.OrdinalIgnoreCase)))
                    {
                        Console.WriteLine($"Update for permission: {permission}");
                        await _PermissionManager.SetForRoleAsync(res.Name, permission, true);
                    }
                }
                return Json(new ResponseWrapper<IdentityRoleDto>().SuccessReponseWrapper(res, "Update new role successful"));
            }
            else
                return Json(new ResponseWrapper<IdentityRoleDto>().ErrorReponseWrapper(default, "Update new role successful", 400));
        }
    }
}
