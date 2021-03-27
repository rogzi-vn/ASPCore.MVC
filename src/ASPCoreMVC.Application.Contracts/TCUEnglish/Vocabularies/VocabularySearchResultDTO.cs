using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.Vocabularies
{
    public class VocabularySearchResultDTO
    {
        public string TopicName { get; set; }
        public string WordClassName { get; set; }
        public string Word { get; set; }
        public string Mean { get; set; }
        public string Pronounce { get; set; }
        public string PronounceAudio { get; set; }
        public string Explain { get; set; }
    }
}
