using ASPCoreMVC.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.UserExams
{
    public class MicroSkillPartDTO : EntityDto<Guid>
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public string Instructions { get; set; }
        public MasterContentTypes MasterContentType { get; set; }
        public bool IsHaveQuestionText { get; set; }
        public TrueAnswerTypes TrueAnswerType { get; set; }
        public AnswerTypes AnswerType { get; set; }
        public bool IsVerticalAnswerDisplay { get; set; } = false;
        public float LimitTimeInMinutes { get; set; }
        public float MaxScores { get; set; }
        /// <summary>
        /// Điểm thực tế người dùng nhận được từ phần thi này
        /// </summary>
        public float Scores { get; set; }
        /// <summary>
        /// Thời điểm mà điểm được cập nhật bởi GVHD, bỏ qua nếu là tự động
        /// </summary>
        public DateTime? UpdateScoreTime { get; set; }
        public List<MicroQuestionContainers> QuestionContainers { get; set; }
    }
}
