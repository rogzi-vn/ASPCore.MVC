using ASPCoreMVC.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.SkillParts
{
    public class CreateUpdateSkillPartDTO : EntityDto<Guid>
    {
        /// <summary>
        /// Mã kỹ năng mà nó thuộc về
        /// </summary>
        [Required]
        public Guid ExamSkillCategoryId { get; set; }
        /// <summary>
        /// Tài nguyên bậc cao dùng chung cho câu hỏi/các câu hỏi
        /// </summary>
        [Required]
        [Display(Name = "Master content type")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value from {1}")]
        public MasterContentTypes MasterContentType { get; set; } = MasterContentTypes.Ignore;
        /// <summary>
        /// Kiểu trả lời
        /// </summary>
        [Required]
        [Display(Name = "Answer type")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value from {1}")]
        public AnswerTypes AnswerType { get; set; } = AnswerTypes.TextAnswer;
        /// <summary>
        /// Thứ tự sắp xếp
        /// </summary>
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value from {1}")]
        public int Order { get; set; } = 0;
        /// <summary>
        /// Tên phần của kỹ năng
        /// </summary>
        [Required]
        [Display(Name = "Name")]
        [MaxLength(255)]
        public string Name { get; set; }
        /// <summary>
        /// Số câu sẽ được hiển thị trong bài thi
        /// </summary>
        [Required]
        [Display(Name = "Number of display")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value from {1}")]
        public int NumDisplay { get; set; }
        /// <summary>
        /// Hướng dẫn mở đầu cho phần làm bài
        /// </summary>
        [Display(Name = "Instructions")]
        public string Instructions { get; set; }
        /// <summary>
        /// Kiểu hiển thị của câu hỏi có phải theo chiều dọc hay không
        /// </summary>
        [Display(Name = "Is Vertical Answer")]
        public bool IsVerticalAnswerDisplay { get; set; } = false;
        /// <summary>
        /// Mẹo làm bài, hiện trước khi người dùng thực sự làm bài
        /// </summary>
        [Display(Name = "Tips")]
        public string Tips { get; set; }
        /// <summary>
        /// Số câu hỏi con
        /// </summary>
        [Required]
        [Display(Name = "Number of sub-question")]
        [Range(-1, int.MaxValue, ErrorMessage = "Please enter a value from {1}")]
        public int NumSubQues { get; set; }
        /// <summary>
        /// Số câu trả lời
        /// </summary>
        [Required]
        [Display(Name = "Number of answer")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value from {1}")]
        public int NumAns { get; set; } = 4;
        /// <summary>
        /// Số phút giới hạn cho phần thi
        /// </summary>
        [Required]
        [Display(Name = "Limit time (minutes)")]
        [Range(0F, float.MaxValue, ErrorMessage = "Please enter a value from {1}")]
        public float LimitTimeInMinutes { get; set; }
        /// <summary>
        /// Số điểm tối đa cho phần thi
        /// </summary>
        [Required]
        [Display(Name = "Max scores (starts)")]
        [Range(1F, float.MaxValue, ErrorMessage = "Please enter a value from {1}")]
        public float MaxScores { get; set; }
        /// <summary>
        /// Loại hiển thị cho editor sửa bài báo,
        /// Chỉ thiển thị khi chọn loại master content là Article
        /// </summary>
        [Required]
        [Display(Name = "Editor layout")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value from {1}")]
        public EditorDisplayOptions ArticleEditor { get; set; } = EditorDisplayOptions.FullOption;
        /// <summary>
        /// Có hay không hiển thị nội dung câu hỏi
        /// </summary>
        [Required]
        [Display(Name = "Display question text")]
        public bool IsHaveQuestionText { get; set; } = true;
        /// <summary>
        /// Kiểu đáp án dúng
        /// </summary>
        [Required]
        [Display(Name = "True answer type")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value from {1}")]
        public TrueAnswerTypes TrueAnswerType { get; set; } = TrueAnswerTypes.OnlyOneCorrect;
    }
}
