using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.ExamCategories
{
    public class SkillPartMinifyDTO : EntityDto<Guid>
    {
        public Guid ExamSkillCategoryId { get; set; }
        public int Order { get; set; } = 0;
        public string Name { get; set; }
    }
}
