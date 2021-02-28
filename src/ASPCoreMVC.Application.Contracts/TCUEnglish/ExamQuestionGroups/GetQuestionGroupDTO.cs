using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.ExamQuestionGroups
{
    public class GetQuestionGroupDTO : PagedAndSortedResultRequestDto
    {
        public Guid SkillPartId { get; set; }
        public string Filter { get; set; }
    }
}
