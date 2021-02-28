using ASPCoreMVC.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace ASPCoreMVC.TCUEnglish._Common.LocalizeContent
{
    public abstract class FullAuditedAggregateRootAndLocalizeContent<T> : FullAuditedAggregateRoot<T>, ILocalizeContent
    {
        public string CultureName { get; set; } = ASPCoreMVCResource.DefaultCulture;
        public Guid RefCode { get; set; } = Guid.NewGuid();
    }
}
