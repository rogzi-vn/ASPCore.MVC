using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace ASPCoreMVC.TCUEnglish.UserMessages
{
    public class UserMessage : AuditedEntity<Guid>
    {
        public Guid MessGroupId { get; set; }
        public string Message { get; set; }
        public bool IsReceived { get; set; }
        public bool IsReaded { get; set; }
    }
}
