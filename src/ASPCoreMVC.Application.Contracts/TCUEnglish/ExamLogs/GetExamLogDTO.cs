using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.ExamLogs
{
    public class GetExamLogDTO : PagedAndSortedResultRequestDto
    {
        public Guid? StudentId { get; set; }
        public Guid? InstructorId { get; set; }
        public string Filter { get; set; }
    }
}
