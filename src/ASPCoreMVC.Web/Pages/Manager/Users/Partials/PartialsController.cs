using ASPCoreMVC.AppUsers;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using ASPCoreMVC.TCUEnglish.ExamCatInstructors;
using ASPCoreMVC.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace ASPCoreMVC.Web.Pages.Manager.Users.Partials
{
    [Authorize]
    [Route("/manager/users")]
    public class PartialsController : AbpController
    {
        private readonly IAppUserService _AppUserService;

        private static string TableView = "~/Pages/Manager/Users/Partials/Table.cshtml";

        public PartialsController(
            IAppUserService _AppUserService)
        {
            this._AppUserService = _AppUserService;
        }

        [Route("display")]
        [HttpGet]
        public async Task<IActionResult> GetAsync(
            [FromQuery] int? p = 1,
            [FromQuery] string filter = "")
        {
            if (p == null || p <= 0) p = 1;
            ViewBag.p = p.Value;

            var serchInp = new GetAppUserDTO
            {
                Filter = filter ?? "",
                MaxResultCount = AppTheme.Limit,
                SkipCount = (p.Value - 1) * AppTheme.Limit
            };
            var res = await _AppUserService.GetListAsync(serchInp);

            if (res.Success)
            {
                PagedResultDto<AppUserDTO> Containers = res.Data;

                string listRes = string.Format("Showing {0} to {1} of {2} entries",
                    res.Data.TotalCount > 0 ? serchInp.SkipCount + 1 : 0, serchInp.SkipCount + res.Data.Items.Count, res.Data.TotalCount);
                if (!filter.IsNullOrEmpty())
                {
                    listRes += string.Format(" for \"{0}\"", serchInp.Filter);
                }
                ViewBag.ListState = listRes;

                ViewBag.Filter = filter;
                ViewBag.Pagination = PaginateHelper.Generate(
                    "javascript:syncVt('{0}', '" + filter + "');",
                    p.Value, Containers.TotalCount, AppTheme.Limit);
                return PartialView(TableView, Containers);
            }
            else
            {
                return PartialView(AppTheme.ContentNothing);
            }
        }

    }
}
