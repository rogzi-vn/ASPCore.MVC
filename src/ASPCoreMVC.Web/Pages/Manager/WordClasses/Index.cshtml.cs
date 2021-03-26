using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCoreMVC.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPCoreMVC.Web.Pages.Manager.WordClasses
{
    [Authorize(ASPCoreMVCPermissions.WordClassesManager.Default)]
    public class WordClassIndexModel : AppPageModel
    {
        public void OnGet()
        {
        }
    }
}
