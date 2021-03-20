using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCoreMVC.TCUEnglish.Notifications;
using ASPCoreMVC.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.Web.Pages.Notifications
{
    public class IndexModel : AppPageModel
    {
        private readonly INotificationService _NotificationService;

        public int CurrentPage { get; set; } = 1;

        public PagedResultDto<NotificationDTO> Notifications { get; set; }

        public string Pagination { get; set; }

        public IndexModel(INotificationService _NotificationService)
        {
            this._NotificationService = _NotificationService;
        }


        public async Task OnGetAsync([FromQuery] int? p = 1)
        {
            CurrentPage = p.Value;
            if (CurrentPage < 1)
                CurrentPage = 1;

            var serchInp = new PagedAndSortedResultRequestDto
            {
                MaxResultCount = AppTheme.Limit,
                SkipCount = (CurrentPage - 1) * AppTheme.Limit
            };

            Notifications = await _NotificationService.GetListAsync(serchInp);

            Pagination = PaginateHelper.Generate(
                "/notifications?p={0}",
                CurrentPage, Notifications.TotalCount, AppTheme.Limit);
        }
    }
}
