using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.Notifications
{
    public class NotificationDTO : AuditedEntityDto<Guid>
    {
        public Guid? TargetUserId { get; set; }
        public string NotificationMessage { get; set; }
        public string HerfLink { get; set; }
        public bool IsChecked { get; set; }
    }
}
