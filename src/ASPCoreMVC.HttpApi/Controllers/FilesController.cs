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
    [Authorize]
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

        [HttpPost]
        [Route("/exams/{examLogId}/audio")]
        public async Task<IActionResult> UploadUserExamAudio([FromForm] IFormFile audio, [FromForm] string fname)
        {
            if (audio == null || audio.Length <= 0 || fname.IsNullOrEmpty())
            {
                return BadRequest();
            }
            return Json(await _AppFileService.PostAudioUploadAsync(new RawAppFileDTO
            {
                Content = await audio.GetAllBytesAsync(),
                Name = fname
            }));
        }
        [HttpPost]
        [Route("/vocabulary/contribute/audio")]
        public async Task<IActionResult> UploadVocabularyAudio([FromForm] IFormFile audio)
        {
            if (audio == null || audio.Length <= 0)
            {
                return BadRequest();
            }
            return Json(await _AppFileService.PostAudioUploadAsync(new RawAppFileDTO
            {
                Content = await audio.GetAllBytesAsync(),
                Name = audio.Name
            }));
        }
    }
}
