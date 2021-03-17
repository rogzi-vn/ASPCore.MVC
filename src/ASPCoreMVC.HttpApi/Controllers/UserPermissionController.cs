using ASPCoreMVC.Models.TreeJs;
using ASPCoreMVC.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.PermissionManagement;

namespace ASPCoreMVC.Controllers
{
    [Authorize]
    [Route("/user-permissions")]
    public class UserPermissionController : AppController
    {
        private readonly IPermissionAppService _PermissionAppService;

        public UserPermissionController(IPermissionAppService _PermissionAppService)
        {
            this._PermissionAppService = _PermissionAppService;
        }

        [Route("treejs-data")]
        [HttpGet]
        public async Task<IActionResult> PermissionsTreeJsDataForRole()
        {
            var treeJsList = new List<TreeJs>();
            var res = await _PermissionAppService.GetAsync("R", "");
            foreach (var g in res.Groups)
            {
                var currentTreeJs = new TreeJs(g.Name, g.DisplayName);
                foreach (var p in g.Permissions)
                {
                    if (p.ParentName.IsNullOrEmpty())
                        currentTreeJs.Children.Add(new TreeJs(p.Name, p.DisplayName));
                    else
                    {
                        for (int i = 0; i < currentTreeJs.Children.Count; i++)
                            if (currentTreeJs.Children[i].Id == p.ParentName)
                                currentTreeJs.Children[i].Children.Add(new TreeJs(p.Name, p.DisplayName));
                    }
                }
                treeJsList.Add(currentTreeJs);
            }
            return Json(treeJsList);
        }
    }
}
