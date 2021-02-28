using ASPCoreMVC.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.SkillParts
{
    public class SkillPartDTO : EntityDto<Guid>
    {
        /// <summary>
        /// Mã kỹ năng mà nó thuộc về
        /// </summary>
        public Guid ExamSkillCategoryId { get; set; }
        /// <summary>
        /// Tài nguyên bậc cao dùng chung cho câu hỏi/các câu hỏi
        /// </summary>
        public MasterContentTypes MasterContentType { get; set; }
        /// <summary>
        /// Kiểu trả lời
        /// </summary>
        public AnswerTypes AnswerType { get; set; }
        /// <summary>
        /// Thứ tự sắp xếp
        /// </summary>
        public int Order { get; set; } = 0;
        /// <summary>
        /// Tên nho nhóm câu hỏi nếu số câu hỏi con > 1
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Số câu sẽ được hiển thị trong bài thi
        /// </summary>
        public int NumDisplay { get; set; }
        /// <summary>
        /// Hướng dẫn mở đầu cho phần làm bài
        /// </summary>
        public string Instructions { get; set; }
        /// <summary>
        /// Mẹo làm bài, hiện trước khi người dùng thực sự làm bài
        /// </summary>
        public string Tips { get; set; }
        /// <summary>
        /// Số câu hỏi con
        /// </summary>
        public int NumSubQues { get; set; }
        /// <summary>
        /// Số câu trả lời
        /// </summary>
        public int NumAns { get; set; }
        /// <summary>
        /// Số phút giới hạn cho phần thi
        /// </summary>
        public float LimitTimeInMinutes { get; set; }
        /// <summary>
        /// Số điểm tối đa cho phần thi
        /// </summary>
        public float MaxScores { get; set; }
        /// <summary>
        /// Loại hiển thị cho editor sửa bài báo,
        /// Chỉ thiển thị khi chọn loại master content là Article
        /// </summary>
        public EditorDisplayOptions ArticleEditor { get; set; }
        /// <summary>
        /// Có hay không hiển thị nội dung câu hỏi
        /// </summary>
        public bool IsHaveQuestionText { get; set; }
        /// <summary>
        /// Kiểu đáp án dúng
        /// </summary>
        public TrueAnswerTypes TrueAnswerType { get; set; }
    }
}
