using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace ASPCoreMVC.TCUEnglish.UserNotes
{
    public class UserNote : AuditedEntity<Guid>
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
