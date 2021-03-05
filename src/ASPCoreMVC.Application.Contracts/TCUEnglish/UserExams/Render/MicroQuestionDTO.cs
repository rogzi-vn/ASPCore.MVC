using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.UserExams
{
    public class MicroQuestionDTO : EntityDto<Guid>
    {
        public string Text { get; set; }
        public List<MicroAnswerDTO> Answers { get; set; }
    }
}
