using ASPCoreMVC.TCUEnglish.UserExams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreMVC.Web.Models
{
    public class QARenderModal
    {
        public int Counter { get; set; }
        public List<MicroQuestionDTO> Questions { get; set; }
        public MicroSkillPartDTO SkillPart { get; set; }

        public QARenderModal(int counter, List<MicroQuestionDTO> questions, MicroSkillPartDTO skillPart)
        {
            Counter = counter;
            Questions = questions;
            SkillPart = skillPart;
        }
    }
}
