using ASPCoreMVC.AppFiles;
using ASPCoreMVC.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Authorization;

namespace ASPCoreMVC.Controllers
{
    public class FilesController : AppController
    {
        private readonly IAppFileService _AppFileService;

        public FilesController(IAppFileService appFileService)
        {
            _AppFileService = appFileService;
        }

        [HttpGet]
        [Route("resources/{**filePath}")]
        public async Task<IActionResult> DownloadUserPhoto(string filePath)
        {
            var truePath = Path.Combine("/", filePath);
            var res = await _AppFileService.GetAppFileAsync(new GetAppFileRequestDTO { Path = truePath });
            if (res.Success)
                return File(res.Data.Content, "application/octet-stream", res.Data.Name);
            else
                return NotFound();
        }
    }
}
