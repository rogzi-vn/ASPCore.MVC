using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.WordClasses
{
    public class WordClassDTO : EntityDto<Guid>
    {
        [Required]
        public string Name { get; set; }
        public string About { get; set; }
    }
}
