using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.ExamLogs
{
    public interface IExamLogService : IWrapperCrudAppService<ExamLogDTO, Guid, GetExamLogDTO>
    {
        public Task<ResponseWrapper<PagedResultDto<ExamLogBaseDTO>>> GetBaseListAsync(GetExamLogDTO input);
    }
}
