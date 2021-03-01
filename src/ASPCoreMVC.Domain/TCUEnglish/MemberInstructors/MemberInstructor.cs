using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace ASPCoreMVC.TCUEnglish.MemberInstructors
{
    public class MemberInstructor : AuditedAggregateRoot<Guid>
    {
        public Guid MemeberId { get; set; }
        public Guid ExamCatInstructorId { get; set; }
        public bool IsInstructorConfirmed { get; set; }
        public bool IsMemberConfirmed { get; set; }
    }
}
