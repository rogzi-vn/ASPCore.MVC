using ASPCoreMVC.TCUEnglish._Common.LocalizeContent;
using System;

namespace ASPCoreMVC.TCUEnglish.ExamSkillCategories
{
    public class ExamSkillCategory : AuditedAggregateRootAndLocalizeContent<Guid>
    {
        public Guid ExamCategoryId { get; set; }
        /// <summary>
        /// Thứ tự sắp xếp
        /// </summary>
        public int Order { get; set; } = 0;
        public string Name { get; set; }
        public string Tips { get; set; }
        public float LimitTimeInMinutes { get; set; }
        public float MaxScores { get; set; }
        public ExamSkillCategory SetId(string guid)
        {
            Id = Guid.Parse(guid);
            return this;
        }
    }
}
