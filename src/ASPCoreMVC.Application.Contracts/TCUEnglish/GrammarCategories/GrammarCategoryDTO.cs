using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.GrammarCategories
{
    public class GrammarCategoryDTO : EntityDto<Guid>
    {
        [Required]
        public string Name { get; set; }
        public string About { get; set; }
    }
}
