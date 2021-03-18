using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ASPCoreMVC.TCUEnglish.Notifications
{
    public interface INotificationService : ICrudAppService<NotificationDTO, Guid>
    {
        public Task UpdateNotificationSeenState(Guid id);
        public Task<int> GetCountUnreadNotification();
    }
}
