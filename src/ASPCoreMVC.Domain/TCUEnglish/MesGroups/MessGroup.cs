using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace ASPCoreMVC.TCUEnglish.MesGroups
{
    public class MessGroup : AuditedEntity<Guid>
    {
        public Guid Starter { get; set; }
        public string GroupName { get; set; }
        public string Members { get; set; }
    }
}
