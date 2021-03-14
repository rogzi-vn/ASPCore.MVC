using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using ASPCoreMVC.TCUEnglish.ScoreLogs;
using ASPCoreMVC.TCUEnglish.UserExams;
using Newtonsoft.Json;
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
        private readonly IRepository<ScoreLog, Guid> ScoreLogRepository;

        public ExamLogService(
            IRepository<ExamLog, Guid> repo,
            IRepository<ScoreLog, Guid> ScoreLogRepository) : base(repo)
        {
            this.ScoreLogRepository = ScoreLogRepository;
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

        public override Task<ResponseWrapper<ExamLogDTO>> UpdateAsync(Guid id, ExamLogDTO input)
        {
            // Cho biết bài thi đã đạt hay chưa
            input.IsPassed = IsExamPassesed(input.ExamScores, input.CurrentMaxScore);
            return base.UpdateAsync(id, input);
        }

        public override Task<ResponseWrapper<ExamLogDTO>> CreateAsync(ExamLogDTO input)
        {
            var exam = JsonConvert.DeserializeObject<ExamForRenderDTO>(input.RawExamRendered);
            if (input.RenderExamType == Common.RenderExamTypes.SkillPart)
            {
                input.CurrentMaxScore = exam.SkillCategories.FirstOrDefault()?.SkillParts?.FirstOrDefault()?.MaxScores ?? 0F;
                input.CurrentMaxTimeInMinutes = exam.SkillCategories.FirstOrDefault()?.SkillParts?.FirstOrDefault()?.LimitTimeInMinutes ?? 0F;
                input.IsPassed = false;
            }
            else if (input.RenderExamType == Common.RenderExamTypes.SkillCategory)
            {
                input.CurrentMaxScore = exam.SkillCategories.FirstOrDefault()?.SkillParts?.Sum(x => x.MaxScores) ?? 0F;
                input.CurrentMaxTimeInMinutes = exam.SkillCategories.FirstOrDefault()?.SkillParts?.Sum(x => x.LimitTimeInMinutes) ?? 0F;
                input.IsPassed = false;
            }
            else if (input.RenderExamType == Common.RenderExamTypes.Synthetic)
            {
                input.CurrentMaxScore = exam.SkillCategories?.Sum(x => x.MaxScores) ?? 0F;
                input.CurrentMaxTimeInMinutes = exam.SkillCategories?.Sum(x => x.LimitTimeInMinutes) ?? 0F;
                input.IsPassed = false;
            }
            input.IsDoneScore = false;
            return base.CreateAsync(input);
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

        public bool IsExamPassesed(float examScores, float examMaxScores)
        {
            return examScores >= examMaxScores / 2;
        }

        private async Task<ExamLog> ExamLogProcessing(ExamLogResultDTO examResult)
        {
            var examLog = await Repository.GetAsync(examResult.LogId);
            if (examLog.CompletionTime != null)
                return null;

            // Tổng điểm của bài thi hiện tại
            var totalScore = 0F;

            // Phân giải thành bài thi đọc được
            var exam = JsonConvert.DeserializeObject<ExamForRenderDTO>(examLog.RawExamRendered);

            // Đánh dấu thời gian hoàn thành bài thi
            examLog.CompletionTime = DateTime.Now;

            // Thời gian thực hiện bài thi
            examLog.ExamTimeInMinutes = (examLog.CompletionTime.Value - examLog.CreationTime).Seconds / 60;

            // Phân giải để thực hiện chấm bài thi
            for (int i = 0; i < exam.SkillCategories.Count; i++)
            {
                // Điểm tối đa của skill category
                var skcMaxScores = exam.SkillCategories[i].MaxScores;
                for (int j = 0; j < exam.SkillCategories[i].SkillParts.Count; j++)
                {
                    var skp = exam.SkillCategories[i].SkillParts[j];

                    // Điểm tối đa của skill part
                    var skpMaxScores = skp.MaxScores;

                    // Điểm cho phần thi thiện tại
                    var skpScores = 0F;

                    // Điểm khả dụng cho mỗi câu hỏi đúng
                    var skpScorePerQuestion = skp.MaxScores /
                        skp.QuestionContainers.SelectMany(x => x.Questions).Count();

                    // Cờ kiểm tra có thể chấm tự động hay không
                    var isAutoCorrectable = skp.AnswerType == Common.AnswerTypes.TextAnswer ||
                               skp.AnswerType == Common.AnswerTypes.ImageAnswer ||
                               skp.AnswerType == Common.AnswerTypes.FillAnswer;


                    #region Bắt đầu phần chấm tự động
                    // Phân giải khung chứa ds câu hỏi
                    for (int k = 0; k < exam
                    .SkillCategories[i]
                    .SkillParts[j]
                    .QuestionContainers
                    .Count; k++)
                    {
                        // Phân giải danh sách câu hỏi
                        for (int l = 0; l < exam
                        .SkillCategories[i]
                        .SkillParts[j]
                        .QuestionContainers[k]
                        .Questions.Count; l++)
                        {
                            // Luu tam cau hoi hien tai
                            var question = exam.SkillCategories[i].SkillParts[j].QuestionContainers[k].Questions[l];

                            // Lay cau tra loi cua nguoi dung cho cau hoi hien tai
                            var userAnswer = examResult.Answers.Find(x => x.QuestionId == question.Id);

                            // Nếu phần này có thể chấm tự động được
                            if (isAutoCorrectable)
                            {
                                //// Nếu nội dung này đã được chấm trước đó thì bỏ qua, vì đây là nội dung chấm tự động
                                //if (skp.QuestionContainers.Any(x => x.Questions.Any(y => y.CorrectionContentTime != null)))
                                //    continue;

                                var isCorrect = false;

                                if (skp.AnswerType == Common.AnswerTypes.FillAnswer)
                                {
                                    // Xử ký KQ cho phần điền
                                    isCorrect = question.Answers.Any(
                                        x => x.AnswerContent.Equals(userAnswer?.Answer ?? "", StringComparison.OrdinalIgnoreCase) &&
                                        x.IsCorrect);
                                }
                                else
                                {
                                    // Xử lý theo kiểu trắc nghiệm
                                    isCorrect = question.Answers.Any(x => x.Id.ToString()
                                    .Equals(userAnswer?.Answer ?? "",
                                    StringComparison.OrdinalIgnoreCase) && x.IsCorrect);
                                }

                                // Luu ket qua dung sai
                                exam.SkillCategories[i]
                                    .SkillParts[j]
                                    .QuestionContainers[k]
                                    .Questions[l].IsCorrect = isCorrect;

                                // Lưu giá trị điểm vào
                                exam.SkillCategories[i]
                                    .SkillParts[j]
                                    .QuestionContainers[k]
                                    .Questions[l].Scores = skpScorePerQuestion;

                                // Lưu thời gian chấm
                                exam.SkillCategories[i]
                                    .SkillParts[j]
                                    .QuestionContainers[k]
                                    .Questions[l].CorrectionContentTime = DateTime.Now;

                            }
                            else
                            {
                                if (exam.SkillCategories[i]
                                    .SkillParts[j]
                                    .QuestionContainers[k]
                                    .Questions[l].IsCorrect)
                                {
                                    exam.SkillCategories[i]
                                    .SkillParts[j]
                                    .QuestionContainers[k]
                                    .Questions[l].CorrectionContentTime = DateTime.Now;
                                }
                            }

                            // Tính điểm nếu câu trả lời này là đúng
                            if (exam.SkillCategories[i]
                                    .SkillParts[j]
                                    .QuestionContainers[k]
                                    .Questions[l].IsCorrect)
                            {
                                totalScore += skpScorePerQuestion;
                                skpScores += skpScorePerQuestion;
                            }
                        }
                    }
                    #endregion

                    // Lưu điểm vào log điểm
                    if (isAutoCorrectable)
                    {
                        // Lưu kết quả chấm vào log Điểm
                        await SaveScoreLog(examLog.Id, skp.Id, skpScores, skpMaxScores);
                    }

                }
            }

            if (!exam.SkillCategories
                .SelectMany(x => x.SkillParts
                .SelectMany(y => y.QuestionContainers
                .SelectMany(z => z.Questions))).Any(x => x.CorrectionContentTime == null))
            {
                // Đánh dấu rằng bài thi đã được chấm hoàn tất
                examLog.IsDoneScore = true;
            }

            examLog.ExamScores = totalScore;
            examLog.UserAnswers = JsonConvert.SerializeObject(examResult.Answers);

            // Chuyển đổi lại thành Json
            examLog.RawExamRendered = JsonConvert.SerializeObject(exam);

            // Cho biết bài thi đã đạt hay chưa
            examLog.IsPassed = IsExamPassesed(examLog.ExamScores, examLog.CurrentMaxScore);

            return examLog;
        }

        public async Task ResultProcessing(ExamLogResultDTO examResult)
        {
            var examLog = await ExamLogProcessing(examResult);
            if (examLog == null)
                return;

            // Cập nhật exam Logs
            await Repository.UpdateAsync(examLog);
        }

        // Lưu lại log điểm của bài kiểm tra
        private async Task SaveScoreLog(Guid examLogId, Guid? destId, float scores, float maxScores)
        {
            if (destId == null || destId == Guid.Empty)
                return;
            var scoreLog = new ScoreLog
            {
                ExamLogId = examLogId,
                DestId = destId.Value,
                Scores = scores,
                MaxScores = maxScores,
                RateInParent = scores / maxScores
            };

            // Xóa các bản đã tổn tại có examLogId và desId trùng đã tồn tại trước đó
            await ScoreLogRepository.DeleteAsync(x => x.ExamLogId == examLogId && x.DestId == destId.Value);
            // Lưu mới vào CSDL
            await ScoreLogRepository.InsertAsync(scoreLog);
        }

        public async Task<int> GetCompletedTest(Guid examCategoryId)
        {
            var query = await Repository.GetQueryableAsync();
            query = query
                .Where(x => x.CreatorId == CurrentUser.Id)
                .Where(x => x.ExamCategoryId == examCategoryId)
                .Where(x => x.CompletionTime != null);
            return query.Count();
        }

        public async Task<int> GetPassedTest(Guid examCategoryId)
        {
            var query = await Repository.GetQueryableAsync();
            query = query
                .Where(x => x.CreatorId == CurrentUser.Id)
                .Where(x => x.ExamCategoryId == examCategoryId)
                .Where(x => x.CompletionTime != null)
                .Where(x => x.IsDoneScore)
                .Where(x => x.IsPassed);
            return query.Count();
        }

        public async Task<int> GetFaildTest(Guid examCategoryId)
        {
            var query = await Repository.GetQueryableAsync();
            query = query
                .Where(x => x.CreatorId == CurrentUser.Id)
                .Where(x => x.ExamCategoryId == examCategoryId)
                .Where(x => x.CompletionTime != null)
                .Where(x => x.IsDoneScore)
                .Where(x => !x.IsPassed);
            return query.Count();
        }

        public async Task<float> GetGPA(Guid examCategoryId)
        {
            var query = await Repository.GetQueryableAsync();
            query = query
                .Where(x => x.CreatorId == CurrentUser.Id)
                .Where(x => x.ExamCategoryId == examCategoryId)
                .Where(x => x.CompletionTime != null)
                .Where(x => x.IsDoneScore)
                .Where(x => !x.IsPassed);
            return query.Count();
        }
    }
}
