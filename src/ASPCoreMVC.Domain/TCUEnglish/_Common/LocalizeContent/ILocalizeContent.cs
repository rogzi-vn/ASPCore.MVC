using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPCoreMVC.TCUEnglish._Common.LocalizeContent
{
    public interface ILocalizeContent
    {
        public string CultureName { get; set; }
        public Guid RefCode { get; set; }
    }
}
