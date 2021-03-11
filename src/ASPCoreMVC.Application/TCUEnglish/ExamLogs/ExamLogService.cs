using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.TCUEnglish.ExamLogs
{
    public class ExamLogService : WrapperCrudAppService<ExamLog, ExamLogDTO, Guid, GetExamLogDTO>,
        IExamLogService
    {
        public ExamLogService(IRepository<ExamLog, Guid> repo) : base(repo)
        {

        }

        protected override async Task<IQueryable<ExamLog>> CreateFilteredQueryAsync(GetExamLogDTO input)
        {
            var query = await Repository.GetQueryableAsync();
            if (input.StudentId != null && input.StudentId != Guid.Empty)
                query = query.Where(x => x.CreatorId == input.StudentId.Value);

            if (input.InstructorId != null && input.InstructorId != Guid.Empty)
                query = query.Where(x => x.ExamCatInstructorId == input.InstructorId.Value);

            if (!input.Filter.IsNullOrEmpty())
                query = query.Where(x => x.RawExamRendered.Contains(input.Filter));

            return query;
        }

        public async Task<ResponseWrapper<PagedResultDto<ExamLogBaseDTO>>> GetBaseListAsync(GetExamLogDTO input)
        {
            var res = await GetListAsync(input);
            return new ResponseWrapper<PagedResultDto<ExamLogBaseDTO>>
            {
                Success = res.Success,
                Message = res.Message,
                ErrorCode = res.ErrorCode,
                Data = new PagedResultDto<ExamLogBaseDTO>(res.Data.TotalCount,
                ObjectMapper.Map<IReadOnlyList<ExamLogDTO>, IReadOnlyList<ExamLogBaseDTO>>(res.Data.Items))
            };
        }

        public Guid? GetLastExamNotFinished()
        {
            return Repository.Where(x => x.CreatorId == CurrentUser.Id &&
           (x.UserAnswers == null || x.UserAnswers.Length == 0)).FirstOrDefault()?.Id ?? null;
        }

        public async Task<ExamLogBaseDTO> GetBaseAsync(Guid id)
        {
            var examLog = await Repository.GetAsync(id);
            return ObjectMapper.Map<ExamLog, ExamLogBaseDTO>(examLog);
        }
    }
}
