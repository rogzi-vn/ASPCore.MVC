using ASPCoreMVC.Common;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace ASPCoreMVC.TCUEnglish.ExamQuestionContainers
{
    public class ExamQuestionContainer : AuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// Nhóm câu hỏi này thuộc phân mục kiểm tra nào
        /// </summary>
        public Guid SkillPartId { get; set; }
        public Guid? ExamQuestionGroupId { get; set; }
        /// <summary>
        /// Mã ngữ pháp mà tập câu hỏi tham chiếu đến
        /// </summary>
        public Guid? GrammarId { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Đường dẫn đã phương tiện (Hình ảnh, âm thanh, video) các phát sẽ phụ thuộc <see cref="QuestionType"/>
        /// </summary>
        public string MediaPath { get; set; }
        /// <summary>
        /// Đoạn văn cho câu hỏi/Có thể là transcript
        /// </summary>
        public string Article { get; set; }

        public ExamQuestionContainer SetId(string guid)
        {
            Id = Guid.Parse(guid);
            return this;
        }
    }
}
