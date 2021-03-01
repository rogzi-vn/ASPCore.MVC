using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.Vocabularies
{
    public class VocabularySimpifyDTO : EntityDto<Guid>
    {
        public string Word { get; set; }
        public string PronounceAudio { get; set; }
    }
}
