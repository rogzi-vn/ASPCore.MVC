using ASPCoreMVC.Localization;
using ASPCoreMVC.TCUEnglish._Common.LocalizeContent;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace ASPCoreMVC.TCUEnglish.ExamCategories
{
    public class ExamCategory : AuditedAggregateRootAndLocalizeContent<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tips { get; set; }

        public ExamCategory SetId(string guid)
        {
            Id = Guid.Parse(guid);
            return this;
        }
    }
}
