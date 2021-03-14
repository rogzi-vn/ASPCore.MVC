using ASPCoreMVC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.UserExams
{
    public class ExamForRenderDTO : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public RenderExamTypes RenderExamType { get; set; }
        public List<MicroSkillCategoryDTO> SkillCategories { get; set; }

        public Guid? GetDesId()
        {
            if (RenderExamType == RenderExamTypes.SkillPart)
            {
                return SkillCategories.FirstOrDefault()?.SkillParts.FirstOrDefault()?.Id;
            }
            else if (RenderExamType == RenderExamTypes.SkillCategory)
            {
                return SkillCategories.FirstOrDefault()?.Id;
            }
            else if (RenderExamType == RenderExamTypes.Synthetic)
            {
                return Id;
            }
            else
            {
                return null;
            }
        }
    }
}
