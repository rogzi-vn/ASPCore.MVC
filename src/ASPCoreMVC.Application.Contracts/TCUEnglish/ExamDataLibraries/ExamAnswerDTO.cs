using ASPCoreMVC.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.ExamDataLibraries
{
    public class ExamAnswerDTO : EntityDto<Guid>
    {
        public Guid ExamQuestionId { get; set; }
        /// <summary>
        /// Nội dung câu trả lời văn bản, media,...
        /// </summary>
        public string AnswerContent { get; set; }
        /// <summary>
        /// Kiểu trả lời
        /// </summary>
        public AnswerTypes AnswerType { get; set; }
        /// <summary>
        /// Cho biết đáp án này đúng hay sai
        /// </summary>
        [Required]
        public bool IsCorrect { get; set; } = false;
        /// <summary>
        /// Kiểu đáp án dúng
        /// </summary>
        public TrueAnswerTypes TrueAnswerType { get; set; }
    }
}
