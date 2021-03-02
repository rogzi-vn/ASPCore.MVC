using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.ExamCatInstructors
{
    public class GetExamCatInstructDTO : PagedAndSortedResultRequestDto
    {
        public Guid? UserId { get; set; }
        public Guid? ExamCategoryId { get; set; }
        public string FilterExamName { get; set; }
        public string FilterDisplayName { get; set; }
    }
}
