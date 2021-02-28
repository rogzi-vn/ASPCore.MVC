using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.ExamCategories
{
    public class ExamCategoryBaseDTO : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
