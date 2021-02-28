using ASPCoreMVC.TCUEnglish._Common.Confirmable;
using ASPCoreMVC.TCUEnglish._Common.LocalizeContent;
using System;

namespace ASPCoreMVC.TCUEnglish.Vocabularies
{
    public class Vocabulary : AuditedAggregateRootAndLocalizeContent<Guid>, IConfirmable
    {
        // Chủ đề từ vựng
        public Guid VocabularyTopicId { get; set; }
        // Phân loại từ (Danh từ, Tính từ,...)
        public Guid WordClassId { get; set; }
        // Từ vựng
        public string Word { get; set; }
        // Giải nghĩa
        public string Explain { get; set; }
        // Phát âm dưới dạng ký tự biểu diễn
        public string Pronounce { get; set; }
        // Phát âm dưới dạng âm thanh
        public string PronounceAudio { get; set; }
        public bool IsConfirmed { get; set; } = false;
        public DateTime? ConfirmedTime { get; set; }
        public Guid? ConfirmerId { get; set; }
    }
}
