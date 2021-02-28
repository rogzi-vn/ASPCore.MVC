using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.AppFiles
{
    public class GetAppFileListDTO : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
