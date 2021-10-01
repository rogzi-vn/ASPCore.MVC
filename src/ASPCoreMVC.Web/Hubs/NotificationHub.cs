using ASPCoreMVC.TCUEnglish.UserMessages;
using ASPCoreMVC.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.Web.Hubs
{
    [HubRoute("/notification-hub")]
    [Authorize]
    public class NotificationHub : AbpHub
    {
        private readonly IReadOnlyRepository<AppUser, Guid> AppUserRepository;

        public NotificationHub(IReadOnlyRepository<AppUser, Guid> AppUserRepository)
        {
            this.AppUserRepository = AppUserRepository;
        }
    }
}