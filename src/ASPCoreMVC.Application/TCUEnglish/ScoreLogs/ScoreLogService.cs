using ASPCoreMVC.Helpers;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using ASPCoreMVC.TCUEnglish.ExamLogs;
using ASPCoreMVC.TCUEnglish.ExamSkillCategories;
using ASPCoreMVC.TCUEnglish.ExamSkillParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.TCUEnglish.ScoreLogs
{
    public class ScoreLogService : ApplicationService, IScoreLogService
    {
        private readonly IRepository<ScoreLog, Guid> ScoreLogRepository;

        private readonly IRepository<ExamLog, Guid> ExamLogRepository;

        private readonly IRepository<ExamSkillCategory, Guid> ExamSkillCategoryRepositoty;
        private readonly IRepository<ExamSkillPart, Guid> ExamSkillPartRepositoty;
        private readonly IRepository<ExamCategory, Guid> ExamCategoryRepository;

        public ScoreLogService(
            IRepository<ScoreLog, Guid> ScoreLogRepository,
            IRepository<ExamSkillCategory, Guid> ExamSkillCategoryRepositoty,
            IRepository<ExamCategory, Guid> ExamCategoryRepository,
            IRepository<ExamSkillPart, Guid> ExamSkillPartRepositoty,
            IRepository<ExamLog, Guid> ExamLogRepository)
        {
            this.ScoreLogRepository = ScoreLogRepository;
            this.ExamSkillCategoryRepositoty = ExamSkillCategoryRepositoty;
            this.ExamCategoryRepository = ExamCategoryRepository;
            this.ExamSkillPartRepositoty = ExamSkillPartRepositoty;
            this.ExamLogRepository = ExamLogRepository;
        }

        public async Task<float> GetExamCategoryGPA(Guid examCategoryGPA)
        {
            return await _GetExamCategoryGPA(examCategoryGPA, null);
        }

        private async Task<float> _GetExamCategoryGPA(Guid examCategoryGPA, DateTime? dest)
        {
            var tempRes = await _GetSkillCategoryGPAs(examCategoryGPA, dest);
            return tempRes.Select(x => x.Scores).DefaultIfEmpty().Sum();
        }

        public async Task<List<DayScoreLogGPADTO>> GetGpaUptoNow(Guid examCatId, DateTime startDate)
        {
            // Lấy danh sách tất cả các ngày từ ngày khởi đầu đến hôm nay
            var dayScoreLogGPAs = new List<DayScoreLogGPADTO>();

            for (var dt = startDate; dt <= DateTime.Now; dt = dt.AddDays(1))
            {
                var item = new DayScoreLogGPADTO
                {
                    Day = dt,
                    DayInString = dt.ToString("dd/MM/yyyy"),
                    GPAScores = await _GetExamCategoryGPA(examCatId, dt)
                };
                dayScoreLogGPAs.Add(item);
            }

            return dayScoreLogGPAs;
        }

        private async Task<float> _GetSkillCategoryGPA(Guid skillCatId, DateTime? dest)
        {
            var skillCat = await ExamSkillCategoryRepositoty.GetAsync(skillCatId);

            var skpQuery = await ExamSkillPartRepositoty.GetQueryableAsync();
            var skillParts = skpQuery.Where(x => x.ExamSkillCategoryId == skillCat.Id)
                .ToList();
            var query = await ScoreLogRepository.GetQueryableAsync();

            if (dest != null)
            {
                query = query.Join(ExamLogRepository,
                        s => s.ExamLogId,
                        e => e.Id,
                        (s, e) => new {s, e})
                    .Where(x => x.e.CreationTime.Year == dest.Value.Year &&
                                x.e.CreationTime.Month == dest.Value.Month &&
                                x.e.CreationTime.Day == dest.Value.Day)
                    .Select(x => x.s);
            }

            return (from skp in skillParts
                let avgRate = query.Where(x => x.CreatorId == CurrentUser.Id)
                    .Where(x => x.DestId == skp.Id)
                    .Select(x => x.RateInParent)
                    .DefaultIfEmpty()
                    .Average()
                select avgRate * skp.MaxScores).Sum();
        }

        public async Task<float> GetSkillCategoryGPA(Guid skillCategoryGPA)
        {
            return await _GetSkillCategoryGPA(skillCategoryGPA, null);
        }

        private async Task<List<SkillCatScoresAvgDTO>> _GetSkillCategoryGPAs(Guid examCategoryGPA, DateTime? dest)
        {
            var res = new List<SkillCatScoresAvgDTO>();

            var examCategory = await ExamCategoryRepository.GetAsync(examCategoryGPA);
            var skillCats = ExamSkillCategoryRepositoty
                .Where(x => x.ExamCategoryId == examCategory.Id)
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var sc in skillCats)
            {
                var scores = await _GetSkillCategoryGPA(sc.Id, dest);
                var scsa = new SkillCatScoresAvgDTO
                {
                    Name = sc.Name,
                    Scores = scores,
                    ScoresRate = scores / sc.MaxScores,
                    HexColor = EzColor.RandomHex()
                };
                res.Add(scsa);
            }

            return res;
        }

        public async Task<List<SkillCatScoresAvgDTO>> GetSkillCategoryGPAs(Guid examCategoryGPA)
        {
            return await _GetSkillCategoryGPAs(examCategoryGPA, null);
        }
    }
}