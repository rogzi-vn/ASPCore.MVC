using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.MessGroups
{
    public class GetMessGroupDTO : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
