using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.ExamCatInstructs
{
    public class ExamCatInstructDTO : EntityDto<Guid>
    {
        public string ExamCategoryName { get; set; }
        public string UserDisplayName { get; set; }
        public Guid UserId { get; set; }
        public Guid ExamCategoryId { get; set; }
    }
}
