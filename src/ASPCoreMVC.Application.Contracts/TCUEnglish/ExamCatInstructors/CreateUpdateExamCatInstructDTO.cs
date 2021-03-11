using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.ExamCatInstructors
{
    public class CreateUpdateExamCatInstructDTO
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid ExamCategoryId { get; set; }
        public bool IsActive { get; set; }
    }
}
