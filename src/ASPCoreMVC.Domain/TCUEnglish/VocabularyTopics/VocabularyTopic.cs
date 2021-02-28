using ASPCoreMVC.TCUEnglish._Common.Confirmable;
using ASPCoreMVC.TCUEnglish._Common.LocalizeContent;
using System;

namespace ASPCoreMVC.TCUEnglish.VocabularyTopics
{
    public class VocabularyTopic : AuditedAggregateRootAndLocalizeContent<Guid>, IConfirmable
    {
        public string Name { get; set; }
        public string About { get; set; }
        public bool IsConfirmed { get; set; } = false;
        public DateTime? ConfirmedTime { get; set; }
        public Guid? ConfirmerId { get; set; }
    }
}
