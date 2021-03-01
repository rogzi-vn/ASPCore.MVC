using ASPCoreMVC.TCUEnglish.GrammarCategories;
using ASPCoreMVC.TCUEnglish.Grammars;
using ASPCoreMVC.TCUEnglish.UserNotes;
using ASPCoreMVC.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace ASPCoreMVC.Web.Pages.Manager.Roles.Partials
{
    [Route("/manager/roles")]
    public class PartialsController : AbpController
    {
        private readonly IIdentityRoleAppService _IdentityRoleAppService;

        private static string CreateUpdateView = "~/Pages/Manager/Roles/Partials/CreateUpdate.cshtml";
        private static string TableView = "~/Pages/Manager/Roles/Partials/Table.cshtml";

        public PartialsController(
            IIdentityRoleAppService _IdentityRoleAppService)
        {
            this._IdentityRoleAppService = _IdentityRoleAppService;
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
            return PartialView(CreateUpdateView);
        }

        [HttpGet]
        [Route("update/{id}")]
        public async Task<IActionResult> GetUpdateAsync(Guid id)
        {
            var res = await _IdentityRoleAppService.GetAsync(id);
            return PartialView(CreateUpdateView, res);
        }
    }
}
