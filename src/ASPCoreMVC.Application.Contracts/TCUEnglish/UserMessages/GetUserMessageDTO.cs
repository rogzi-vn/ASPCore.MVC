using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.UserMessages
{
    public class GetUserMessageDTO : PagedAndSortedResultRequestDto
    {
        public Guid MessGroupId { get; set; }
        public Guid OldestUserMessageId { get; set; }
        public string Filter { get; set; }
    }
}
