using System.ComponentModel.DataAnnotations;

namespace ASPCoreMVC.AppFiles
{
    public class GetAppFileRequestDTO
    {
        [Required]
        public string Path { get; set; }
    }
}
