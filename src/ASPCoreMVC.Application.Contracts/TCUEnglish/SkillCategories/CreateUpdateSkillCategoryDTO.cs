using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.SkillCategories
{
    public class CreateUpdateSkillCategoryDTO : EntityDto<Guid>
    {
        [Required]
        public Guid ExamCategoryId { get; set; }
        /// <summary>
        /// Thứ tự sắp xếp
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int Order { get; set; } = 0;
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }
        public string Tips { get; set; }
        [Range(0, float.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public float LimitTimeInMinutes { get; set; }
        [Range(0, float.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public float MaxScores { get; set; }
    }
}
