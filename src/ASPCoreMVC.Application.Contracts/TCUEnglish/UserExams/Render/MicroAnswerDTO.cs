using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.UserExams
{
    public class MicroAnswerDTO : EntityDto<Guid>
    {
        public string AnswerContent { get; set; }
        public bool IsCorrect { get; set; }
    }
}
