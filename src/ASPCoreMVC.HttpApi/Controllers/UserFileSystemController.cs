using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using elFinder.NetCore;
using elFinder.NetCore.Drivers.FileSystem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.MultiTenancy;

namespace ASPCoreMVC.Controllers
{
    [Authorize]
    [Route("el-finder-user-file-system")]
    public class UserFileSystemController : AppController
    {
        private readonly IWebHostEnvironment _env;
        public UserFileSystemController(IWebHostEnvironment env) => _env = env;

        [HttpGet, HttpPost, IgnoreAntiforgeryToken]
        [Route("connector")]
        public async Task<IActionResult> Connector()
        {
            var connector = ExtractConnector();
            return await connector.ProcessAsync(Request);
        }

        [Route("thumb/{hash}")]
        [HttpGet]
        public async Task<IActionResult> Thumbs(string hash)
        {
            var connector = ExtractConnector();
            return await connector.GetThumbnailAsync(HttpContext.Request, HttpContext.Response, hash);
        }

        protected Connector ExtractConnector()
        {
            string pathroot = Path.Combine(CurrentTenant.Name ?? "host", "all_users", CurrentUser.UserName);

            var driver = new FileSystemDriver();

            string absoluteUrl = UriHelper.BuildAbsolute(Request.Scheme, Request.Host);
            var uri = new Uri(absoluteUrl);

            string rootDirectory = Path.Combine(_env.WebRootPath, pathroot);

            string url = $"{uri.Scheme}://{uri.Authority}/{pathroot}/";
            string urlthumb = $"{uri.Scheme}://{uri.Authority}/el-finder-user-file-system/thumb/";


            var root = new RootVolume(rootDirectory, url, urlthumb)
            {
                //IsReadOnly = !User.IsInRole("Administrators")
                IsReadOnly = false, // Can be readonly according to user's membership permission
                IsLocked = false, // If locked, files and directories cannot be deleted, renamed or moved
                Alias = CurrentUser.UserName, // Beautiful name given to the root/home folder
                MaxUploadSizeInMb = 30, // Limit imposed to user uploaded file <= 2048 KB
                //LockedFolders = new List<string>(new string[] { "Folder1" }
                ThumbnailSize = 80,
            };


            driver.AddRoot(root);

            return new Connector(driver)
            {
                // This allows support for the "onlyMimes" option on the client.
                MimeDetect = MimeDetectOption.Internal
            };
        }
    }
}
