using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.GrammarCategories
{
    public class GrammarCategoryBaseDTO : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
