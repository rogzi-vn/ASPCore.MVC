using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.ExamDataLibraries
{
    public class ExamQuestionDTO : EntityDto<Guid>
    {
        /// <summary>
        /// Nhóm của câu hỏi (nếu có)
        /// </summary>
        public Guid? ExamQuestionContainerId { get; set; }
        /// <summary>
        /// Nội dung câu hỏi dạng văn bản
        /// </summary>
        [Required]
        public string Text { get; set; }
        public string TextClone { get; set; }
        /// <summary>
        /// Giải thích cho đáp án của câu hỏi
        /// </summary>
        public string Explain { get; set; }
        /// <summary>
        /// Có hay không hiển thị nội dung câu hỏi
        /// </summary>
        public bool IsHaveQuestionText { get; set; }
        public List<ExamAnswerDTO> Answers { get; set; }

        public ExamQuestionDTO SetId(string guid)
        {
            return SetId(Guid.Parse(guid));
        }
        public ExamQuestionDTO SetId(Guid guid)
        {
            Id = guid;
            return this;
        }
    }
}
