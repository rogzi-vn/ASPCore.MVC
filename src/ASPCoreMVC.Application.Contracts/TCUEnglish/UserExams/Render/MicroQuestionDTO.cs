using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.UserExams
{
    public class MicroQuestionDTO : EntityDto<Guid>
    {
        public string Text { get; set; }
        public string TextClone { get; set; }
        /// <summary>
        /// Nội dung chỉnh sửa cho đúng bởi GVHD
        /// </summary>
        public string CorrectContentByInstructor { get; set; }
        /// <summary>
        /// Thời điểm mà GVHD chỉnh sửa nội dung
        /// </summary>
        public DateTime? CorrectionContentTime { get; set; }
        public List<MicroAnswerDTO> Answers { get; set; }
    }
}
