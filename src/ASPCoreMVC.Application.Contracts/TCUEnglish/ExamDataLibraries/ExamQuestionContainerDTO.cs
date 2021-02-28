using ASPCoreMVC.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.ExamDataLibraries
{
    public class ExamQuestionContainerDTO : EntityDto<Guid>
    {
        /// <summary>
        /// Nhóm câu hỏi này thuộc phân mục kiểm tra nào
        /// </summary>
        public Guid SkillPartId { get; set; }
        /// <summary>
        /// Tài nguyên bậc cao dùng chung cho câu hỏi/các câu hỏi
        /// </summary>
        public MasterContentTypes MasterContentType { get; set; }
        /// <summary>
        /// Loại hiển thị cho editor sửa bài báo,
        /// Chỉ thiển thị khi chọn loại master content là Article
        /// </summary>
        public EditorDisplayOptions ArticleEditor { get; set; }
        public Guid? ExamQuestionGroupId { get; set; }
        /// <summary>
        /// Mã ngữ pháp mà tập câu hỏi tham chiếu đến
        /// </summary>
        [DisplayName("Reference Grammar")]
        public Guid? GrammarId { get; set; }
        [DisplayName("Question name")]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Đường dẫn đã phương tiện (Hình ảnh, âm thanh, video) các phát sẽ phụ thuộc <see cref="QuestionType"/>
        /// </summary>
        public string MediaPath { get; set; }
        /// <summary>
        /// Đoạn văn cho câu hỏi/Có thể là transcript
        /// </summary>
        public string Article { get; set; }

        public List<ExamQuestionDTO> Questions { get; set; }

        public ExamQuestionContainerDTO SetId(string guid)
        {
            return SetId(Guid.Parse(guid));
        }
        public ExamQuestionContainerDTO SetId(Guid guid)
        {
            Id = guid;
            return this;
        }
    }
}
