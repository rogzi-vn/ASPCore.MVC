using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.ExamDataLibraries
{
    public class GetExamContainerDTO : PagedAndSortedResultRequestDto
    {
        public Guid SkillPartId { get; set; }
        public Guid? QuestionGroupId { get; set; }
        public string Filter { get; set; }
    }
}
