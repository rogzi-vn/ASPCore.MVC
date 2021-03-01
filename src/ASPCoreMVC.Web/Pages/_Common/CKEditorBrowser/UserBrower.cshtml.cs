using ASPCoreMVC.Helpers;
using Microsoft.AspNetCore.Hosting;

namespace ASPCoreMVC.Web.Pages._Common.CKEditorBrowser
{
    public class CKEditorUserBrowerIndexModel : AppPageModel
    {
        private readonly IWebHostEnvironment _env;
        public CKEditorUserBrowerIndexModel(IWebHostEnvironment env) => _env = env;
        public void OnGet()
        {
            ViewData["RootDirectory"] = PathHelper.TrueCombine(CurrentTenant.Name ?? "host", "all_users", CurrentUser.UserName);
        }
    }
}
