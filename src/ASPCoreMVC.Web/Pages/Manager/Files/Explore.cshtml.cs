using ASPCoreMVC.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace ASPCoreMVC.Web.Pages.Manager.Files
{
    [Authorize]
    public class FileExploreModel : AppPageModel
    {
        private readonly IWebHostEnvironment _env;
        public FileExploreModel(IWebHostEnvironment env) => _env = env;
        public void OnGet()
        {
            ViewData["RootDirectory"] = PathHelper.TrueCombine(CurrentTenant.Name ?? "host", "user_files");
        }
    }
}
