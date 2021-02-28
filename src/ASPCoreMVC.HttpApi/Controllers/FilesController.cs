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

        [HttpPost]
        [Route("files/upload/photo")]
        public async Task<IActionResult> UploadPhotoAsync(IFormFile file, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (file != null && file.Length > 0)
            {
                if (!file.IsImage())
                {
                    return Json(new { uploaded = false, message = "Khong co anh" });
                }
                else
                {
                    try
                    {
                        // Nếu người dùng đã chọn ảnh đại diện mới
                        using var memoryStream = new MemoryStream();
                        await file.CopyToAsync(memoryStream);

                        var AppFile = await _AppFileService.PostPhotoUploadAsync(
                            new RawAppFileDTO
                            {
                                Name = file.FileName,
                                Content = memoryStream.ToArray(),
                            }
                        );
                        // Cập nhật đường dẫn ảnh vào cho hồ sơ người dùng
                        return Json(new { uploaded = true, url = PathHelper.TrueCombine("/resources", AppFile.Data.Path) });
                    }
                    catch (AbpAuthorizationException)
                    {
                        return Unauthorized();
                    }
                }
            }
            return Json(new { uploaded = false, message = "Co loi" });
        }
    }
}
