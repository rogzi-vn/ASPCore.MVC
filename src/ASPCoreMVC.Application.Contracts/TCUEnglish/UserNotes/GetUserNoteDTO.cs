using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.UserNotes
{
    public class GetUserNoteDTO : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
