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

namespace ASPCoreMVC.Web.Pages.Dictionary.Notes.Partials
{
    [Route("/notes")]
    public class PartialsController : AbpController
    {
        private readonly IUserNoteService _UserNoteService;

        private static string CreateUpdateView = "~/Pages/Notes/Partials/CreateUpdate.cshtml";
        private static string TableView = "~/Pages/Notes/Partials/Table.cshtml";

        public PartialsController(
            IUserNoteService _UserNoteService)
        {
            this._UserNoteService = _UserNoteService;
        }

        [Route("display")]
        [HttpGet]
        public async Task<IActionResult> GetAsync(
            [FromQuery] int? p = 1,
            [FromQuery] string filter = "")
        {
            if (p == null || p <= 0) p = 1;
            ViewBag.p = p.Value;

            var serchInp = new GetUserNoteDTO
            {
                Filter = filter ?? "",
                MaxResultCount = AppTheme.Limit,
                SkipCount = (p.Value - 1) * AppTheme.Limit
            };
            var res = await _UserNoteService.GetBaseListAsync(serchInp);

            if (res.Success)
            {
                PagedResultDto<UserNoteBaseDTO> Containers = res.Data;

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

        [HttpGet]
        [Route("create")]
        public IActionResult GetCreateAsync()
        {
            return PartialView(CreateUpdateView, new UserNoteDTO());
        }

        [HttpGet]
        [Route("update/{id}")]
        public async Task<IActionResult> GetUpdateAsync(Guid id)
        {
            var res = await _UserNoteService.GetAsync(id);
            if (res.Success)
                return PartialView(CreateUpdateView, res.Data);
            else
                return PartialView(AppTheme.ContentNothing);
        }

        [HttpGet]
        [Route("{id:Guid}/detail")]
        public async Task<IActionResult> Detail(Guid id)
        {
            if (id == Guid.Empty)
                return PartialView(AppTheme.ContentNothing);

            var res = await _UserNoteService.GetAsync(id);
            if (res.Success && res.Data != null)
                return PartialView("~/Pages/Notes/Partials/Detail.cshtml", res.Data);
            else
                return PartialView(AppTheme.ContentNothing);
        }
    }
}
