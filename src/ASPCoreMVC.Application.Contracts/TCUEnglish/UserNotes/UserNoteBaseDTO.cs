using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.UserNotes
{
    public class UserNoteBaseDTO : EntityDto<Guid>
    {
        public string Title { get; set; }
    }
}
