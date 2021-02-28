using ASPCoreMVC.Localization;
using ASPCoreMVC.TCUEnglish._Common.LocalizeContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace ASPCoreMVC.TCUEnglish.GrammarCategories
{
    public class GrammarCategory : AuditedAggregateRootAndLocalizeContent<Guid>
    {
        public string Name { get; set; }
        public string About { get; set; }
    }
}
