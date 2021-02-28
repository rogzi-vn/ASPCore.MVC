using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.AppUsers
{
    public class GetAppUserDTO : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
