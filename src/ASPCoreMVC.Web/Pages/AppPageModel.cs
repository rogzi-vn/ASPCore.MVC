using ASPCoreMVC.Localization;
using ASPCoreMVC.Web.Helpers;
using ASPCoreMVC.Web.Models;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace ASPCoreMVC.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class AppPageModel : AbpPageModel
    {
        protected AppPageModel()
        {
            LocalizationResourceType = typeof(ASPCoreMVCResource);
        }

        public AbpPageModel ToastSuccess(string message)
        {
            return ToastHelper.ToastSuccess(this, message);
        }

        public AbpPageModel ToastError(string message)
        {
            return ToastHelper.ToastError(this, message);
        }

        public AbpPageModel ToastWarning(string message)
        {
            return ToastHelper.ToastWarning(this, message);
        }

        public AbpPageModel ToastInfo(string message)
        {
            return ToastHelper.ToastInfo(this, message);
        }

        public AbpPageModel SetBreadcrumb(params string[][] inp)
        {
            ViewData["Breadcrumb"] = BreadcrumbVM.BuildBreadcrumb(inp);
            return this;
        }
        public AbpPageModel SetBreadcrumbBtn(string name, string href, string icon = "fas fa-plus", string c = "btn-primary")
        {
            ViewData["BreadcrumbBtnHref"] = href;
            ViewData["BreadcrumbBtnClass"] = c;
            ViewData["BreadcrumbBtnText"] = name;
            ViewData["BreadcrumbBtnIcon"] = icon;
            return this;
        }
    }
}