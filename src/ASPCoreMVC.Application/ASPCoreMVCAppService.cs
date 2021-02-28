using System;
using System.Collections.Generic;
using System.Text;
using ASPCoreMVC.Localization;
using Volo.Abp.Application.Services;

namespace ASPCoreMVC
{
    /* Inherit your application services from this class.
     */
    public abstract class ASPCoreMVCAppService : ApplicationService
    {
        protected ASPCoreMVCAppService()
        {
            LocalizationResource = typeof(ASPCoreMVCResource);
        }
    }
}
