using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.Vocabularies
{
    public class VocabularyDTO : EntityDto<Guid>
    {
        [Required]
        // Chủ đề từ vựng
        public Guid VocabularyTopicId { get; set; }
        [Required]
        // Phân loại từ (Danh từ, Tính từ,...)
        public Guid WordClassId { get; set; }
        [Required]
        // Từ vựng
        public string Word { get; set; }
        // Giải nghĩa
        public string Explain { get; set; }
        [Required]
        // Phát âm dưới dạng ký tự biểu diễn
        public string Pronounce { get; set; }
        // Phát âm dưới dạng âm thanh
        public string PronounceAudio { get; set; }
        public bool IsConfirmed { get; set; } = false;
        public DateTime? ConfirmedTime { get; set; }
        public Guid? ConfirmerId { get; set; }
    }
}
