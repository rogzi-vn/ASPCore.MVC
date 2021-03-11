using ASPCoreMVC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace ASPCoreMVC.TCUEnglish.ExamLogs
{
    public class ExamLog : AuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// Kiểu render
        /// </summary>
        public RenderExamTypes RenderExamType { get; set; }
        /// <summary>
        /// Mã của mục thi sẽ được render
        /// </summary>
        public Guid DestinationId { get; set; }
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
