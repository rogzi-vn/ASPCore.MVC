using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ASPCoreMVC.TCUEnglish.ScoreLogs
{
    public interface IScoreLogService : IApplicationService
    {
        public Task<float> GetSkillCategoryGPA(Guid skillCategoryGPA);
        public Task<List<SkillCatScoresAvgDTO>> GetSkillCategoryGPAs(Guid examCategoryGPA);
        public Task<float> GetExamCategoryGPA(Guid examCategoryGPA);
        public Task<List<DayScoreLogGPADTO>> GetGpaUptoNow(Guid examCatId, DateTime startDate);
    }
}
