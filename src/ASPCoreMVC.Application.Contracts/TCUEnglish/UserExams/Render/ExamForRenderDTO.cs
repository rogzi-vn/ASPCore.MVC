using ASPCoreMVC.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPCoreMVC.TCUEnglish.UserExams
{
    public class ExamForRenderDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public RenderExamTypes RenderExamType { get; set; }
        public List<MicroSkillCategoryDTO> SkillCategories { get; set; }
    }
}
