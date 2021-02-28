using ASPCoreMVC.Common;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace ASPCoreMVC.TCUEnglish.ExamQuestionGroups
{
    public class ExamQuestionGroup : AuditedAggregateRoot<Guid>
    {
        public Guid SkillPartId { get; set; }
        public string Name { get; set; }

        public ExamQuestionGroup SetId(string guid)
        {
            Id = Guid.Parse(guid);
            return this;
        }
    }
}
