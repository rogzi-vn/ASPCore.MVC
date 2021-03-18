using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace ASPCoreMVC.TCUEnglish.Notifications
{
    public class Notification : AuditedEntity<Guid>
    {
        public Guid? TargetUserId { get; set; }
        public string NotificationMessage { get; set; }
        public string HerfLink { get; set; }
        public bool IsChecked { get; set; }
    }
}
