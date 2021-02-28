using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.Grammars
{
    public class GetGrammarDTO : PagedAndSortedResultRequestDto
    {
        public Guid? GrammarCategoryId { get; set; }
        public string Filter { get; set; }
    }
}
