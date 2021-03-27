using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.Vocabularies
{
    public class QuickVocabularyTestDTO : EntityDto<Guid>
    {
        public string Vocabulary { get; set; }
        public string Mean { get; set; }
        public List<string> Answers { get; set; }
    }
}
