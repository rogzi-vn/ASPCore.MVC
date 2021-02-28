using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPCoreMVC.Web.Pages
{
    [Authorize]
    public class IndexModel : AppPageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("/dashboard/index");
        }
    }
}