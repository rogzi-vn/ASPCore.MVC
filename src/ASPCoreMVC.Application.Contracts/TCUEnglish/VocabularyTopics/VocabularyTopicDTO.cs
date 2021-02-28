using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.VocabularyTopics
{
    public class VocabularyTopicDTO : EntityDto<Guid>
    {
        [Required]
        public string Name { get; set; }
        public string About { get; set; }
        public bool IsConfirmed { get; set; } = false;
        public DateTime? ConfirmedTime { get; set; }
        public Guid? ConfirmerId { get; set; }
    }
}
