using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.Vocabularies
{
    public class GetVocabularyDTO : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        // Chủ đề từ vựng
        public Guid? VocabularyTopicId { get; set; }
        // Phân loại từ (Danh từ, Tính từ,...)
        public Guid? WordClassId { get; set; }
        public bool IsMustConfirm { get; set; } = true;
    }
}
