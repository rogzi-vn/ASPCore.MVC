using ASPCoreMVC.TCUEnglish.ExamDataLibraries;
using ASPCoreMVC.TCUEnglish.SkillParts;
using ASPCoreMVC.TCUEnglish.UserExams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreMVC.Web.Helpers
{
    public static class ExamQuestionConvertHelper
    {
        public static List<MicroQuestionDTO> Convert(this ICollection<ExamQuestionDTO> data)
        {
            // Xử lý phần câu hỏi
            var res = new List<MicroQuestionDTO>();
            foreach (var q in data)
            {
                var mq = new MicroQuestionDTO
                {
                    Id = q.Id,
                    Text = q.Text,
                    TextClone = q.TextClone,
                    Explain = q.Explain,
                    Answers = q.Answers.Convert()
                };
                res.Add(mq);
            }
            return res;
        }

        public static List<MicroAnswerDTO> Convert(this ICollection<ExamAnswerDTO> data)
        {
            // Xử lý phần câu hỏi
            var res = new List<MicroAnswerDTO>();
            foreach (var x in data)
            {
                var mq = new MicroAnswerDTO
                {
                    Id = x.Id,
                    AnswerContent = x.AnswerContent,
                    IsCorrect = x.IsCorrect
                };
                res.Add(mq);
            }
            return res;
        }
        public static MicroSkillPartDTO Convert(this SkillPartDTO skillPart)
        {
            return new MicroSkillPartDTO
            {
                Id = skillPart.Id,
                Order = skillPart.Order,
                Name = skillPart.Name,
                Instructions = skillPart.Instructions,
                MasterContentType = skillPart.MasterContentType,
                IsHaveQuestionText = skillPart.IsHaveQuestionText,
                TrueAnswerType = skillPart.TrueAnswerType,
                AnswerType = skillPart.AnswerType,
                LimitTimeInMinutes = skillPart.LimitTimeInMinutes,
                MaxScores = skillPart.MaxScores,
                IsVerticalAnswerDisplay = skillPart.IsVerticalAnswerDisplay
            };
        }

        public static MicroSkillPartDTO Convert(this SkillPartDTO skillPart, ExamQuestionContainerDTO container)
        {
            var res = skillPart.Convert();
            res.QuestionContainers = new List<MicroQuestionContainers>
            {
                new MicroQuestionContainers
                {
                    Id = container.Id,
                    Name = container.Name,
                    MediaPath = container.MediaPath
                }
            };
            return res;
        }

    }
}
