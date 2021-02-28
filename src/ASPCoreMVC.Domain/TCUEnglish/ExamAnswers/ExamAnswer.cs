using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace ASPCoreMVC.TCUEnglish.ExamAnswers
{
    public class ExamAnswer : AuditedAggregateRoot<Guid>
    {
        public Guid ExamQuestionId { get; set; }
        /// <summary>
        /// Nội dung câu trả lời văn bản, media,...
        /// </summary>
        public string AnswerContent { get; set; }
        /// <summary>
        /// Cho biết đáp án này đúng hay sai
        /// </summary>
        public bool IsCorrect { get; set; }
    }
}
