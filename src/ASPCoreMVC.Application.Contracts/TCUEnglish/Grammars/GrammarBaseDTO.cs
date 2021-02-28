using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.Grammars
{
    public class GrammarBaseDTO : EntityDto<Guid>
    {
        [Required]
        public Guid GrammarCategoryId { get; set; }
        /// <summary>
        /// Tên ngữ pháp
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Cấu trúc của ngữ pháp
        /// </summary>
        [Required]
        public string Structure { get; set; }
    }
}
