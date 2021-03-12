using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
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

        public async Task ResultProcessing(ExamLogResultDTO examResult)
        {
            var examLog = await Repository.GetAsync(examResult.LogId);
            if (examLog.CompletionTime != null)
                return;
            var totalScore = 0F;
            var exam = JsonConvert.DeserializeObject<ExamForRenderDTO>(examLog.RawExamRendered);
            examLog.CompletionTime = DateTime.Now;
            examLog.ExamTimeInMinutes = (examLog.CreationTime - examLog.CompletionTime.Value).Minutes;
            for (int i = 0; i < exam.SkillCategories.Count; i++)
            {
                var skcMaxScores = exam.SkillCategories[i].MaxScores;
                for (int j = 0; j < exam.SkillCategories[i].SkillParts.Count; j++)
                {
                    var skp = exam.SkillCategories[i].SkillParts[j];
                    var skpMaxScores = skp.MaxScores;
                    var skpScorePerQuestion = skp.MaxScores /
                        skp.QuestionContainers.SelectMany(x => x.Questions).Count();
                    for (int k = 0; k < exam
                        .SkillCategories[i]
                        .SkillParts[j]
                        .QuestionContainers
                        .Count; k++)
                    {
                        for (int l = 0; l < exam
                        .SkillCategories[i]
                        .SkillParts[j]
                        .QuestionContainers[k]
                        .Questions.Count; l++)
                        {
                            // Luu tam cau hoi hien tai
                            var question = exam
                                .SkillCategories[i]
                                .SkillParts[j]
                                .QuestionContainers[k]
                                .Questions[l];

                            // Lay cau tra loi cua nguoi dung cho cau hoi hien tai
                            var userAnswer = examResult.Answers.Find(x => x.QuestionId == question.Id);

                            if (skp.AnswerType == Common.AnswerTypes.TextAnswer ||
                               skp.AnswerType == Common.AnswerTypes.ImageAnswer ||
                               skp.AnswerType == Common.AnswerTypes.FillAnswer)
                            {
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

                                if (isCorrect)
                                {
                                    totalScore += skpScorePerQuestion;
                                }

                                // Luu ket qua dung sai
                                exam
                                    .SkillCategories[i]
                                    .SkillParts[j]
                                    .QuestionContainers[k]
                                    .Questions[l].IsCorrect = isCorrect;

                                // Lưu thời gian chấm
                                exam
                                    .SkillCategories[i]
                                    .SkillParts[j]
                                    .QuestionContainers[k]
                                    .Questions[l].CorrectionContentTime = DateTime.Now;
                            }
                        }
                    }
                }
            }

            examLog.ExamScores = totalScore;
            examLog.UserAnswers = JsonConvert.SerializeObject(examResult.Answers);

            // Chuyển đổi lại thành Json
            examLog.RawExamRendered = JsonConvert.SerializeObject(exam);

            // Cho biết bài thi đã đạt hay chưa
            examLog.IsPassed = IsExamPassesed(examLog.ExamScores, examLog.CurrentMaxScore);

            // Cập nhật exam Logs
            await Repository.UpdateAsync(examLog);
        }
    }
}
