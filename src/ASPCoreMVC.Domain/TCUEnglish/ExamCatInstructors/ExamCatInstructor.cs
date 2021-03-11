using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace ASPCoreMVC.TCUEnglish.ExamCatInstructors
{
    public class ExamCatInstructor : AuditedAggregateRoot<Guid>
    {
        public Guid UserId { get; set; }
        public Guid ExamCategoryId { get; set; }
        public bool IsActive { get; set; }
    }
}
