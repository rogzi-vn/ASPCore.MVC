using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.ExamCategories
{
    public class CreateUpdateExamCategoryDTO : EntityDto<Guid>
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }
        public string Tips { get; set; }
    }
}
