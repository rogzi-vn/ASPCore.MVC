using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.SkillCategories
{
    public class SkillCategoryBaseDTO : EntityDto<Guid>
    {
        public Guid ExamCategoryId { get; set; }
        /// <summary>
        /// Thứ tự sắp xếp
        /// </summary>
        public int Order { get; set; } = 0;
        public string Name { get; set; }
        public float LimitTimeInMinutes { get; set; }
        public float MaxScores { get; set; }
    }
}
