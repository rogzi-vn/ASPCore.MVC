using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using ASPCoreMVC.AppFiles;
using ASPCoreMVC.Helpers;
using ASPCoreMVC.Permissions;
using ASPCoreMVC.TCUEnglish._Common.BlobStorages;
using ASPCoreMVC.TCUEnglish.AppFiles;
using Microsoft.AspNetCore.Authorization;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.AppFileService
{
    [Authorize]
    public class AppFileService : WrapperCrudAppService<
        AppFile,
        AppFileDTO,
        Guid,
        GetAppFileListDTO>, IAppFileService
    {
        private readonly IBlobContainer<UserFileContainer> _fileContainer;
        public AppFileService(
            IRepository<AppFile, Guid> repository,
            IBlobContainer<UserFileContainer> fileContainer) : base(repository)
        {
            _fileContainer = fileContainer;
            GetPolicyName = ASPCoreMVCPermissions.AppFiles.Default;
            GetListPolicyName = ASPCoreMVCPermissions.AppFiles.Default;
            CreatePolicyName = ASPCoreMVCPermissions.AppFiles.Create;
            UpdatePolicyName = ASPCoreMVCPermissions.AppFiles.Edit;
            DeletePolicyName = ASPCoreMVCPermissions.AppFiles.Delete;
        }

        [AllowAnonymous]
        public async Task<ResponseWrapper<AppFileResponseDTO>> GetAppFileAsync(GetAppFileRequestDTO input)
        {
            var fileName = Path.GetFileName(input.Path);
            var blob = await _fileContainer.GetAllBytesAsync(fileName);

            // Nếu filename lớn hơn 37 thì tiến hành sub để loại bỏ GUID
            if (fileName.Length > 37)
            {
                fileName = fileName[37..];
            }

            return new ResponseWrapper<AppFileResponseDTO>(
                new AppFileResponseDTO
                {
                    Name = fileName,
                    Content = blob
                }, "Successful");
        }

        private async Task<ResponseWrapper<AppFileDTO>> SaveRawFile(RawAppFileDTO input, AppFile parent
            , string successMessage = "Upload file successful")
        {
            var res = new ResponseWrapper<AppFileDTO>(default, "", 500);
            if (parent == null ||
                parent.Id == Guid.Empty ||
                parent.Path.IsNullOrEmpty())
            {
                res.Message = "Not found parent folder";
                res.ErrorCode = 404;
                return res;
            }

            if (!parent.IsDirectory)
            {
                res.Message = "Parent must be a Directory";
                res.ErrorCode = 403;
                return res;
            }

            Guid appFileId = Guid.NewGuid();
            string newFileName = appFileId.ToString() + "_" + input.Name;

            // Tạo đối tượng cho file mới
            var file = new AppFile
            {
                ParentId = parent.Id,
                Name = newFileName,
                IsDirectory = false,
                Length = input.Content.Length
            }.SetPath(PathHelper.TrueCombine(parent.Path, newFileName))
            .SetId(appFileId);
            // Cập nhật vào CSDL
            var uploaded = await Repository.InsertAsync(file);
            try
            {
                // Lưu file
                await _fileContainer.SaveAsync(newFileName, input.Content, true);
                return new ResponseWrapper<AppFileDTO>(
                    new AppFileDTO
                    {
                        ParentId = uploaded.Id,
                        Id = uploaded.Id,
                        IsDirectory = uploaded.IsDirectory,
                        Length = uploaded.Length,
                        Name = uploaded.Name,
                        Path = uploaded.Path
                    }, successMessage);
            }
            catch (Exception e)
            {
                // Nếu thất bại thì xóa bản ghi trong csdl đi
                await Repository.DeleteAsync(file.Id);
                return new ResponseWrapper<AppFileDTO>(default, e.Message, 500);
            }
        }

        public async Task<ResponseWrapper<AppFileDTO>> PostAudioUploadAsync(RawAppFileDTO input)
        {
            return await SaveRawFile(
                input,
                AppFileDefaults.AppAudiosDirectory,
                "Upload audio successful");
        }

        public async Task<ResponseWrapper<AppFileDTO>> PostCommonUploadAsync(RawAppFileDTO input)
        {
            return await SaveRawFile(
                input,
                AppFileDefaults.AppCommonsDirectory);
        }

        public async Task<ResponseWrapper<AppFileDTO>> PostPhotoUploadAsync(RawAppFileDTO input)
        {
            return await SaveRawFile(
                            input,
                            AppFileDefaults.AppPhotosDirectory,
                            "Upload photo successful");
        }

        public async Task<ResponseWrapper<AppFileDTO>> PostRootUploadAsync(RawAppFileDTO input)
        {
            return await SaveRawFile(
                input,
                AppFileDefaults.RootDirectory);
        }

        public async Task<ResponseWrapper<AppFileDTO>> PostUserAvatarUploadAsync(RawAppFileDTO input)
        {
            return await SaveRawFile(
                input,
                AppFileDefaults.UserAvatarsDirectory,
                "Upload user avatar successful");
        }

        public async Task<ResponseWrapper<AppFileDTO>> PostVideoUploadAsync(RawAppFileDTO input)
        {
            return await SaveRawFile(
                input,
                AppFileDefaults.AppVideosDirectory,
                "Upload video successful");
        }
    }
}
