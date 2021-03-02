using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using ASPCoreMVC.AppUsers;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.ExamCatInstructors
{
    public interface IExamCatInstructService : IWrapperCrudAppService<
        ExamCatInstructDTO,
        Guid,
        GetExamCatInstructDTO,
        CreateUpdateExamCatInstructDTO>
    {
        public Task<ResponseWrapper<PagedResultDto<AppUserDTO>>> GetInstructorsAsync(Guid ExamCatInstructorId, GetAppUserDTO input);
    }
}
