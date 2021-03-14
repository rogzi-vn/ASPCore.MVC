using ASPCoreMVC._Commons;
using ASPCoreMVC.Common;
using ASPCoreMVC.TCUEnglish.ExamAnswers;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using ASPCoreMVC.TCUEnglish.ExamQuestionContainers;
using ASPCoreMVC.TCUEnglish.ExamQuestions;
using ASPCoreMVC.TCUEnglish.ExamSkillCategories;
using ASPCoreMVC.TCUEnglish.ExamSkillParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.TCUEnglish.UserExams
{
    public class RenderExamService : ApplicationService, IRenderExamService
    {
        private readonly IRepository<ExamCategory, Guid> _ExamCategoryRepository;
        private readonly IRepository<ExamSkillCategory, Guid> _ExamSkillCategoryRepository;
        private readonly IRepository<ExamSkillPart, Guid> _ExamSkillPartRepository;
        private readonly IRepository<ExamQuestionContainer, Guid> _ExamQuestionContainerRepository;
        private readonly IRepository<ExamQuestion, Guid> _ExamQuestionRepository;
        private readonly IRepository<ExamAnswer, Guid> _ExamAnswerRepository;

        public RenderExamService(
            IRepository<ExamCategory, Guid> _ExamCategoryRepository,
            IRepository<ExamSkillCategory, Guid> _ExamSkillCategoryRepository,
            IRepository<ExamSkillPart, Guid> _ExamSkillPartRepository,
            IRepository<ExamQuestionContainer, Guid> _ExamQuestionContainerRepository,
            IRepository<ExamQuestion, Guid> _ExamQuestionRepository,
            IRepository<ExamAnswer, Guid> _ExamAnswerRepository)
        {
            this._ExamCategoryRepository = _ExamCategoryRepository;
            this._ExamSkillCategoryRepository = _ExamSkillCategoryRepository;
            this._ExamSkillPartRepository = _ExamSkillPartRepository;
            this._ExamQuestionContainerRepository = _ExamQuestionContainerRepository;
            this._ExamQuestionRepository = _ExamQuestionRepository;
            this._ExamAnswerRepository = _ExamAnswerRepository;
        }

        public async Task<ResponseWrapper<ExamForRenderDTO>> GetRenderExam(RenderExamTypes type, Guid? destID)
        {
            var res = new ResponseWrapper<ExamForRenderDTO>()
                .ErrorReponseWrapper(null, "Error", 304);
            var examForRender = new ExamForRenderDTO();

            if (type == RenderExamTypes.SkillPart)
            {
                // Process skill part
                var skillPart = await _ExamSkillPartRepository.GetAsync(destID.Value);
                if (skillPart == null)
                    return res;
                var micSkillPart = new List<MicroSkillPartDTO> { await ConvertRenderSkillPart(skillPart) };

                // Process skill cat
                var skillCat = await _ExamSkillCategoryRepository.GetAsync(skillPart.ExamSkillCategoryId);
                var micSkillCat = new List<MicroSkillCategoryDTO>
                {
                    new MicroSkillCategoryDTO
                    {
                        Id = skillCat.Id,
                        Order = skillCat.Order,
                        Name = skillCat.Name,
                        LimitTimeInMinutes = skillCat.LimitTimeInMinutes,
                        MaxScores = skillCat.MaxScores,
                        SkillParts = micSkillPart
                    }
                };

                // Process exam
                var examCat = await _ExamCategoryRepository.GetAsync(skillCat.ExamCategoryId);
                examForRender = new ExamForRenderDTO
                {
                    Id = examCat.Id,
                    Name = examCat.Name,
                    Description = examCat.Description,
                    RenderExamType = type,
                    SkillCategories = micSkillCat
                };
            }
            else if (type == RenderExamTypes.SkillCategory)
            {
                // Process kill cat
                var skillCat = await _ExamSkillCategoryRepository.GetAsync(destID.Value);
                if (skillCat == null)
                    return res;
                var micSkillCat = new List<MicroSkillCategoryDTO> { await ConvertRenderSkillCategory(skillCat) };
                // Process exam
                var examCat = await _ExamCategoryRepository.GetAsync(skillCat.ExamCategoryId);
                examForRender = new ExamForRenderDTO
                {
                    Id = examCat.Id,
                    Name = examCat.Name,
                    Description = examCat.Description,
                    RenderExamType = type,
                    SkillCategories = micSkillCat
                };
            }
            else if (type == RenderExamTypes.Synthetic)
            {
                examForRender = await RenderExamCategoryFromId(destID.Value, type);
            }
            else
                return res;
            return new ResponseWrapper<ExamForRenderDTO>()
                .SuccessReponseWrapper(examForRender, "Success");
        }

        private async Task<ExamForRenderDTO> ConvertExamForRender(ExamCategory examCat, RenderExamTypes type)
        {
            var query = await _ExamSkillCategoryRepository.GetQueryableAsync();
            var skillCats = query
                .Where(x => x.ExamCategoryId == examCat.Id)
                .ToList();
            var microSkillCats = new List<MicroSkillCategoryDTO>();
            foreach (var item in skillCats)
            {
                var converted = await ConvertRenderSkillCategory(item);
                microSkillCats.Add(converted);
            }
            // Sắp xếp chuẩn
            microSkillCats = microSkillCats.OrderBy(x => x.Order).ToList();
            return new ExamForRenderDTO
            {
                Id = examCat.Id,
                Name = examCat.Name,
                Description = examCat.Description,
                RenderExamType = type,
                SkillCategories = microSkillCats
            };
        }
        private async Task<ExamForRenderDTO> RenderExamCategoryFromId(Guid id, RenderExamTypes type)
        {
            // Get skill part by Id
            var res = await _ExamCategoryRepository.GetAsync(id);
            if (res == null)
                return null;
            return await ConvertExamForRender(res, type);
        }

        private async Task<MicroSkillCategoryDTO> ConvertRenderSkillCategory(ExamSkillCategory skillCat)
        {
            var query = await _ExamSkillPartRepository.GetQueryableAsync();
            var skillParts = query
                .Where(x => x.ExamSkillCategoryId == skillCat.Id)
                .ToList();
            var microSkillParts = new List<MicroSkillPartDTO>();
            foreach (var item in skillParts)
            {
                var converted = await ConvertRenderSkillPart(item);
                microSkillParts.Add(converted);
            }
            // Sắp xếp lại các phần cho đúng thứ tự
            microSkillParts = microSkillParts.OrderBy(x => x.Order).ToList();
            return new MicroSkillCategoryDTO
            {
                Id = skillCat.Id,
                Order = skillCat.Order,
                Name = skillCat.Name,
                LimitTimeInMinutes = skillCat.LimitTimeInMinutes,
                MaxScores = skillCat.MaxScores,
                SkillParts = microSkillParts
            };
        }
        private async Task<MicroSkillCategoryDTO> RenderSkillCategoryFromId(Guid id)
        {
            // Get skill part by Id
            var res = await _ExamSkillCategoryRepository.GetAsync(id);
            if (res == null)
                return null;
            return await ConvertRenderSkillCategory(res);
        }

        private async Task<MicroSkillPartDTO> ConvertRenderSkillPart(ExamSkillPart skillPart)
        {
            // Get answer containers of skill part size by NumDisplay
            var qContainers = await GetRandomQuestionContainers(skillPart.Id, skillPart.NumDisplay);
            for (int i = 0; i < qContainers.Count; i++)
            {
                qContainers[i].Questions = await GetQuestions(qContainers[i].Id);
                for (int j = 0; j < qContainers[i].Questions.Count; j++)
                {
                    qContainers[i].Questions[j].Answers = await GetAnswers(qContainers[i].Questions[j].Id);
                }
            }
            return new MicroSkillPartDTO
            {
                Id = skillPart.Id,
                Order = skillPart.Order,
                Name = skillPart.Name,
                Instructions = skillPart.Instructions,
                MasterContentType = skillPart.MasterContentType,
                IsHaveQuestionText = skillPart.IsHaveQuestionText,
                TrueAnswerType = skillPart.TrueAnswerType,
                AnswerType = skillPart.AnswerType,
                LimitTimeInMinutes = skillPart.LimitTimeInMinutes,
                MaxScores = skillPart.MaxScores,
                IsVerticalAnswerDisplay = skillPart.IsVerticalAnswerDisplay,
                QuestionContainers = qContainers
            };
        }
        private async Task<MicroSkillPartDTO> RenderSkillPartFromId(Guid skillPartId)
        {
            // Get skill part by Id
            var skillPart = await _ExamSkillPartRepository.GetAsync(skillPartId);
            if (skillPart == null)
                return null;
            return await ConvertRenderSkillPart(skillPart);
        }

        /// <summary>
        /// Phương thức lấy ngẫu nhiên các câu hỏi
        /// </summary>
        /// <param name="skillPartId">Mã phần thi</param>
        /// <param name="numDisplay">Số lượng bộ chứa câu hỏi muốn hiển thị</param>
        /// <returns></returns>
        private async Task<List<MicroQuestionContainers>> GetRandomQuestionContainers(Guid skillPartId, int numDisplay)
        {
            var query = await _ExamQuestionContainerRepository
                .GetQueryableAsync();
            // Chuẩn xác chuyển sang query dành cho skill part đúng với thực tế
            query = query.Where(x => x.SkillPartId == skillPartId);

            // Khai báo biến chứa danh sách kết quả
            var records = new List<MicroQuestionContainers>();

            // Lấy cho đủ số câu hỏi
            Random rand = new Random();
            for (int i = 0; i < numDisplay; i++)
            {
                var tempQuery = query;

                // Không lấy những record đã tồn tại trong danh sách kết quả
                foreach (var record in records)
                    tempQuery = tempQuery.Where(x => x.Id != record.Id);

                int skip = rand.Next(0, tempQuery.Count());

                var tempRes = tempQuery
                    .Skip(skip)
                    .Take(1)
                    .Select(x => new MicroQuestionContainers
                    {
                        Id = x.Id,
                        Name = x.Name,
                        MediaPath = x.MediaPath
                    })
                    .First();
                records.Add(tempRes);
            }

            return records;
        }

        private async Task<List<MicroQuestionDTO>> GetQuestions(Guid questionContainerId)
        {
            var query = await _ExamQuestionRepository.GetQueryableAsync();
            return query
                .Where(x => x.ExamQuestionContainerId == questionContainerId)
                .Select(x => new MicroQuestionDTO
                {
                    Id = x.Id,
                    Text = x.Text,
                    TextClone = x.TextClone
                }).ToList();
        }

        private async Task<List<MicroAnswerDTO>> GetAnswers(Guid questionId)
        {
            var query = await _ExamAnswerRepository.GetQueryableAsync();
            return query
                .Where(x => x.ExamQuestionId == questionId)
                .Select(x => new MicroAnswerDTO
                {
                    Id = x.Id,
                    AnswerContent = x.AnswerContent,
                    IsCorrect = x.IsCorrect
                }).ToList()
                .OrderBy(x => Guid.NewGuid())
                .ToList();
        }

        public async Task<string> GetRenderArtical(Guid containerId)
        {
            var qc = await _ExamQuestionContainerRepository.GetAsync(containerId);
            return qc?.Article ?? "";
        }
    }
}
