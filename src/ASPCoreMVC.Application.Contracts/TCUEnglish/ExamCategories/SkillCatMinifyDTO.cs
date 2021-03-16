using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.ExamCategories
{
    public class SkillCatMinifyDTO : EntityDto<Guid>
    {
        public Guid ExamCategoryId { get; set; }
        public int Order { get; set; } = 0;
        public string Name { get; set; }
        public List<SkillPartMinifyDTO> SkillParts { get; set; }
    }
}
