using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.Grammars
{
    public class GrammarSimpify : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
