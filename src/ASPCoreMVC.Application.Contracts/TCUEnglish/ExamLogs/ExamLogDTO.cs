using ASPCoreMVC.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.ExamLogs
{
    public class ExamLogDTO : EntityDto<Guid>
    {
        /// <summary>
        /// Kiểu render
        /// </summary>
        public RenderExamTypes RenderExamType { get; set; }
        public Guid DestinationId { get; set; }
        public Guid ExamCategoryId { get; set; }
        /// <summary>
        /// Tên bài thi
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Nội dung được render
        /// </summary>
        public string RawExamRendered { get; set; }
        /// <summary>
        /// Nội dung câu trả lời của người dùng
        /// </summary>
        public string UserAnswers { get; set; }
        /// <summary>
        /// Điểm tối đa cho bài thi thiện tại
        /// </summary>
        public float CurrentMaxScore { get; set; }
        /// <summary>
        /// Bài thi đã vượt qua hay chưa
        /// </summary>
        public bool IsPassed { get; set; }
        /// <summary>
        /// Bài thi đã chấm điểm hay chưa
        /// </summary>
        public bool IsDoneScore { get; set; }
        /// <summary>
        /// Thời gian tối đã cho phần thi hiện tại
        /// </summary>
        public float CurrentMaxTimeInMinutes { get; set; }
        /// <summary>
        /// Thời gian mà người dùng đã sử dụng để làm bài thi này
        /// </summary>
        public float ExamTimeInMinutes { get; set; }
        /// <summary>
        /// Điểm của bài thi này
        /// </summary>
        public float ExamScores { get; set; }
        /// <summary>
        /// Mã GVHD kèm vào nếu có
        /// </summary>
        public Guid? ExamCatInstructorId { get; set; }
        /// <summary>
        /// Lời nhận xét của GVHD
        /// </summary>
        public string InstructorComments { get; set; }
        /// <summary>
        /// Thời điểm bài thi kết thúc
        /// </summary>
        public DateTime? CompletionTime { get; set; }
        /// <summary>
        /// Thời điểm GVHD cập nhật
        /// </summary>
        public DateTime? InstructorCompletionTime { get; set; }
    }
}
