using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.WordClasses
{
    public class GetWordClassDTO : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
