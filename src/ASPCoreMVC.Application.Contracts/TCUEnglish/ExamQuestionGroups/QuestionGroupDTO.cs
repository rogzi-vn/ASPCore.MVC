using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.ExamQuestionGroups
{
    public class QuestionGroupDTO : EntityDto<Guid>
    {
        [Required]
        public Guid SkillPartId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
