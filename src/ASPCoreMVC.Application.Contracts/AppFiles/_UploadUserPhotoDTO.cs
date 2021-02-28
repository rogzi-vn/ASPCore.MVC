using ASPCoreMVC;
using ASPCoreMVC.Helpers.CustomAttributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ASPCoreMVC.AppFiles
{
    public class _UploadUserPhotoDTO
    {
        [DataType(DataType.Upload)]
        [MaxFileSize(AppContractConsts.MaxFileSizeInBytes, ErrorMessage = "Image size too big")]
        [AllowedExtensions(new string[] { ".jpg", ".png" }, ErrorMessage = "Please upload image only")]
        public IFormFile File { get; set; }
        public string Name { get; set; }
    }
}
