using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ASPCoreMVC.AppFiles
{
    // https://docs.abp.io/en/abp/latest/Blob-Storing
    // https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types
    public interface IAppFileService : IWrapperCrudAppService<AppFileDTO, Guid, GetAppFileListDTO>
    {
        Task<ResponseWrapper<AppFileDTO>> PostRootUploadAsync(RawAppFileDTO input);
        Task<ResponseWrapper<AppFileDTO>> PostUserAvatarUploadAsync(RawAppFileDTO input);
        Task<ResponseWrapper<AppFileDTO>> PostPhotoUploadAsync(RawAppFileDTO input);
        Task<ResponseWrapper<AppFileDTO>> PostVideoUploadAsync(RawAppFileDTO input);
        Task<ResponseWrapper<AppFileDTO>> PostAudioUploadAsync(RawAppFileDTO input);
        Task<ResponseWrapper<AppFileDTO>> PostCommonUploadAsync(RawAppFileDTO input);
        Task<ResponseWrapper<AppFileResponseDTO>> GetAppFileAsync(GetAppFileRequestDTO input);
        
    }
}
