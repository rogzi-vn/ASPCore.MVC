using ASPCoreMVC.Helpers;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using ASPCoreMVC.TCUEnglish.ExamSkillCategories;
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

        private readonly IRepository<ExamSkillCategory, Guid> ExamSkillCategoryRepositoty;
        private readonly IRepository<ExamCategory, Guid> ExamCategoryRepository;

        public ScoreLogService(
            IRepository<ScoreLog, Guid> ScoreLogRepository,
            IRepository<ExamSkillCategory, Guid> ExamSkillCategoryRepositoty,
            IRepository<ExamCategory, Guid> ExamCategoryRepository)
        {
            this.ScoreLogRepository = ScoreLogRepository;
            this.ExamSkillCategoryRepositoty = ExamSkillCategoryRepositoty;
            this.ExamCategoryRepository = ExamCategoryRepository;
        }

        public async Task<float> GetExamCategoryGPA(Guid examCategoryGPA)
        {
            var tempRes = await GetSkillCategoryGPAs(examCategoryGPA);
            return tempRes.Sum(x => x.Scores);
        }

        public async Task<float> GetSkillCategoryGPA(Guid skillCategoryGPA)
        {
            var skillCat = await ExamSkillCategoryRepositoty.GetAsync(skillCategoryGPA);
            var query = await ScoreLogRepository.GetQueryableAsync();
            return query
                .Where(x => x.DestId == skillCategoryGPA)
                .Sum(x => x.Scores / x.MaxScores * skillCat.MaxScores);
        }

        public async Task<List<SkillCatScoresAvgDTO>> GetSkillCategoryGPAs(Guid examCategoryGPA)
        {
            var res = new List<SkillCatScoresAvgDTO>();

            var examCategory = await ExamCategoryRepository.GetAsync(examCategoryGPA);
            var skillCats = ExamSkillCategoryRepositoty
                .Where(x => x.ExamCategoryId == examCategory.Id)
                .OrderBy(x => x.Order)
                .ToList();
            foreach (var sc in skillCats)
            {
                var scores = await GetSkillCategoryGPA(sc.Id);
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
    }
}
