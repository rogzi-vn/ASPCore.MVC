using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.UserExams
{
    public class MicroSkillCategoryDTO : EntityDto<Guid>
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public float LimitTimeInMinutes { get; set; }
        public float MaxScores { get; set; }
        public List<MicroSkillPartDTO> SkillParts { get; set; }
    }
}
