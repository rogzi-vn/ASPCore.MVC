using ASPCoreMVC.Helpers;
using Microsoft.AspNetCore.Hosting;

namespace ASPCoreMVC.Web.Pages._Common.CKEditorBrowser
{
    public class CKEditorBrowserIndexModel : AppPageModel
    {
        private readonly IWebHostEnvironment _env;
        public CKEditorBrowserIndexModel(IWebHostEnvironment env) => _env = env;
        public void OnGet()
        {
            ViewData["RootDirectory"] = PathHelper.TrueCombine(CurrentTenant.Name ?? "host", "user_files");
        }
    }
}
