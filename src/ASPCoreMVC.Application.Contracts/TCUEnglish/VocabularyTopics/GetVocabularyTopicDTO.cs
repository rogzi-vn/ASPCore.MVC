using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.VocabularyTopics
{
    public class GetVocabularyTopicDTO : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
