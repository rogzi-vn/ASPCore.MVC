using ASPCoreMVC.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ASPCoreMVC.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class AppController : AbpController
    {
        protected AppController()
        {
            LocalizationResource = typeof(ASPCoreMVCResource);
        }
    }
}