using ASPCoreMVC.Common;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace ASPCoreMVC.TCUEnglish.ExamQuestions
{
    public class ExamQuestion : AuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// Nhóm của câu hỏi (nếu có)
        /// </summary>
        public Guid? ExamQuestionContainerId { get; set; }
        /// <summary>
        /// Nội dung câu hỏi dạng văn bản
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Giải thích cho đáp án của câu hỏi
        /// </summary>
        public string Explain { get; set; }

        public ExamQuestion SetId(string guid)
        {
            Id = Guid.Parse(guid);
            return this;
        }
    }
}
