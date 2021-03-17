using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.Vocabularies
{
    public class GetSearchVocabularyDTO : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public bool IsMustConfirm { get; set; } = true;
    }
}
