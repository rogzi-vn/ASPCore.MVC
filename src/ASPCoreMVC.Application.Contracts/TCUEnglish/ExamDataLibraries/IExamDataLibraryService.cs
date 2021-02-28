using ASPCoreMVC._Commons;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ASPCoreMVC.TCUEnglish.ExamDataLibraries
{
    public interface IExamDataLibraryService : IApplicationService
    {
        public Task<ResponseWrapper<ExamQuestionContainerDTO>> GetForCreateAsync(Guid skPartId);
        public Task<ResponseWrapper<bool>> GetIsHaveAnyAsync(Guid skPartId);
        public Task<ResponseWrapper<ExamQuestionContainerDTO>> GetForUpdateAsync(Guid containerId);
        public Task<ResponseWrapper<ExamQuestionContainerDTO>> PostSyncAsync(ExamQuestionContainerDTO inp);
        public Task<ResponseWrapper<PagedResultDto<ExamQuestionContainerDTO>>> GetListAsync(GetExamContainerDTO inp);
        public Task DeleteAsync(Guid id);
    }
}
