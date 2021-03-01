using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.ExamCatInstructs
{
    public interface IExamCatInstructService : IWrapperCrudAppService<
        ExamCatInstructDTO,
        Guid,
        GetExamCatInstructDTO,
        CreateUpdateExamCatInstructDTO>
    {
        public Task<ResponseWrapper<PagedResultDto<ExamCatInstructDTO>>> GetAllExamCatInstruct(GetExamCatInstructDTO inp);
    }
}
