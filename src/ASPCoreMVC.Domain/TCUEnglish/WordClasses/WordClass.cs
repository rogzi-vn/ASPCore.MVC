using ASPCoreMVC.TCUEnglish._Common.LocalizeContent;
using System;

namespace ASPCoreMVC.TCUEnglish.WordClasses
{
    public class WordClass : AuditedAggregateRootAndLocalizeContent<Guid>
    {
        public string Name { get; set; }
        public string About { get; set; }
    }
}
