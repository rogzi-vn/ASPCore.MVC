using ASPCoreMVC._Commons;
using ASPCoreMVC.TCUEnglish.ExamAnswers;
using ASPCoreMVC.TCUEnglish.ExamQuestionContainers;
using ASPCoreMVC.TCUEnglish.ExamQuestionGroups;
using ASPCoreMVC.TCUEnglish.ExamQuestions;
using ASPCoreMVC.TCUEnglish.ExamSkillParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.TCUEnglish.ExamDataLibraries
{
    public class ExamDataLibraryService : ApplicationService, IExamDataLibraryService
    {
        private readonly IRepository<ExamSkillPart, Guid> _ExamSkillPartRepository;
        private readonly IRepository<ExamQuestionContainer, Guid> _ExamQuestionContainerRepository;
        private readonly IRepository<ExamQuestion, Guid> _ExamQuestionRepository;
        private readonly IRepository<ExamAnswer, Guid> _ExamAnswerRepository;

        public ExamDataLibraryService(
            IRepository<ExamSkillPart, Guid> _ExamSkillPartRepository,
            IRepository<ExamQuestionContainer, Guid> _ExamQuestionContainerRepository,
            IRepository<ExamQuestion, Guid> _ExamQuestionRepository,
            IRepository<ExamAnswer, Guid> _ExamAnswerRepository)
        {
            this._ExamSkillPartRepository = _ExamSkillPartRepository;
            this._ExamQuestionContainerRepository = _ExamQuestionContainerRepository;
            this._ExamQuestionRepository = _ExamQuestionRepository;
            this._ExamAnswerRepository = _ExamAnswerRepository;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _ExamQuestionContainerRepository.DeleteAsync(id);
        }

        public async Task<ResponseWrapper<ExamQuestionContainerDTO>> GetForCreateAsync(Guid skPartId)
        {
            var res = new ResponseWrapper<ExamQuestionContainerDTO>().ErrorReponseWrapper(default, "Error", -100);
            // Get parent (Skill part) to get all neccessary properties for config 
            var skP = await _ExamSkillPartRepository.GetAsync(skPartId);
            if (skP == null)
            {
                // Return error immediately
                res.Message = "Skil part not found";
                return res;
            }
            // Genera container base on properties of skP
            var containerId = Guid.NewGuid();
            var container = new ExamQuestionContainerDTO
            {
                Id = containerId,
                SkillPartId = skP.Id,
                MasterContentType = skP.MasterContentType,
                ArticleEditor = skP.ArticleEditor,

            };
            // Generate question list
            var questions = new List<ExamQuestionDTO>();
            for (int i = 0; i < skP.NumSubQues; i++)
            {
                // Generate question
                var questionId = Guid.NewGuid();
                var question = new ExamQuestionDTO
                {
                    Id = questionId,
                    ExamQuestionContainerId = containerId,
                    IsHaveQuestionText = skP.IsHaveQuestionText
                };

                // Generate answers list
                var answers = new List<ExamAnswerDTO>();
                for (int j = 0; j < skP.NumAns; j++)
                {
                    answers.Add(new ExamAnswerDTO
                    {
                        ExamQuestionId = questionId,
                        AnswerType = skP.AnswerType,
                        TrueAnswerType = skP.TrueAnswerType
                    });
                }

                // Add answers to question
                question.Answers = answers;

                // Add questions to collection
                questions.Add(question);
            }

            // Add questions to container
            container.Questions = questions;

            // Config response again
            res = res.SuccessReponseWrapper(container, "Successful");
            return res;
        }

        public async Task<ResponseWrapper<ExamQuestionContainerDTO>> GetForUpdateAsync(Guid containerId)
        {
            var res = new ResponseWrapper<ExamQuestionContainerDTO>().ErrorReponseWrapper(default, "Error", -100);
            // Get question container
            var _container = await _ExamQuestionContainerRepository.GetAsync(containerId);
            if (_container == null)
            {
                // Return error immediately
                res.Message = "Question not found";
                return res;
            }
            var container = ObjectMapper.Map<ExamQuestionContainer, ExamQuestionContainerDTO>(_container);
            // Get parent (Skill part) to get all neccessary properties for config 
            var skP = await _ExamSkillPartRepository.GetAsync(container.SkillPartId);
            if (skP == null)
            {
                // Return error immediately
                res.Message = "Skil part not found";
                return res;
            }
            container.MasterContentType = skP.MasterContentType;
            container.ArticleEditor = skP.ArticleEditor;
            // Get questions
            var questions = ObjectMapper.Map<
                List<ExamQuestion>,
                List<ExamQuestionDTO>>(_ExamQuestionRepository
                .Where(x => x.ExamQuestionContainerId == container.Id)
                .ToList());
            // If question not enough by config, add new
            for (int k = 0; k < skP.NumSubQues - questions.Count; k++)
            {
                // Generate question
                var questionId = Guid.NewGuid();
                var question = new ExamQuestionDTO
                {
                    ExamQuestionContainerId = containerId,
                    IsHaveQuestionText = skP.IsHaveQuestionText
                }.SetId(questionId);
            }
            // Get answers per question
            for (int i = 0; i < questions.Count; i++)
            {
                // Get answers
                var answers = ObjectMapper.Map<
                    List<ExamAnswer>,
                    List<ExamAnswerDTO>>(
                    _ExamAnswerRepository
                    .Where(x => x.ExamQuestionId == questions[i].Id)
                    .ToList());
                // If answers not enough by config, add new
                for (int j = 0; j < skP.NumAns - answers.Count; j++)
                {
                    answers.Add(new ExamAnswerDTO
                    {
                        ExamQuestionId = questions[i].Id,
                        AnswerType = skP.AnswerType,
                        TrueAnswerType = skP.TrueAnswerType
                    });
                }
                // Set answer to question
                questions[i].Answers = answers;
                questions[i].IsHaveQuestionText = skP.IsHaveQuestionText;
            }
            // Add question to container
            container.Questions = questions;
            // Config response again
            res = res.SuccessReponseWrapper(container, "Successful");
            return res;
        }

        public async Task<ResponseWrapper<bool>> GetIsHaveAnyAsync(Guid skPartId)
        {
            return new ResponseWrapper<bool>()
                .SuccessReponseWrapper(await _ExamQuestionContainerRepository
                .AnyAsync(x => x.SkillPartId == skPartId), "Successful");
        }

        public async Task<ResponseWrapper<PagedResultDto<ExamQuestionContainerDTO>>> GetListAsync(GetExamContainerDTO inp)
        {
            var query = await _ExamQuestionContainerRepository.GetQueryableAsync();
            query = query.Where(x => x.SkillPartId == inp.SkillPartId);
            if (inp.QuestionGroupId != null && inp.QuestionGroupId != Guid.Empty)
            {
                query = query.Where(x => x.ExamQuestionGroupId == inp.QuestionGroupId.Value);
            }
            if (!inp.Filter.IsNullOrEmpty())
            {
                query = query.Where(x => x.Name.Contains(inp.Filter));
            }
            if (!inp.Sorting.IsNullOrEmpty())
            {
                query = query
                .OrderBy(inp.Sorting);
            }
            var queryResult = query
                .Skip(inp.SkipCount)
                .Take(inp.MaxResultCount);
            return new ResponseWrapper<PagedResultDto<ExamQuestionContainerDTO>>()
                 .SuccessReponseWrapper(new PagedResultDto<ExamQuestionContainerDTO>(query.Count(),
                 ObjectMapper.Map<
                     List<ExamQuestionContainer>,
                     List<ExamQuestionContainerDTO>>(queryResult.ToList())), "Successful");
        }

        public async Task<ResponseWrapper<ExamQuestionContainerDTO>> PostSyncAsync(ExamQuestionContainerDTO inp)
        {
            var res = new ResponseWrapper<ExamQuestionContainerDTO>().ErrorReponseWrapper(default, "Error", -100);
            if (inp.ExamQuestionGroupId == Guid.Empty)
            {
                inp.ExamQuestionGroupId = null;
            }
            // sync container
            if (await _ExamQuestionContainerRepository.AnyAsync(x => x.Id == inp.Id))
            {
                // update container case
                var entity = await _ExamQuestionContainerRepository.GetAsync(inp.Id);
                if (entity == null)
                {
                    res.Message = "Not found question container";
                    return res;
                }
                //TODO: Check if input has id different than given id and normalize if it's default value, throw ex otherwise
                ObjectMapper.Map(inp, entity);
                await _ExamQuestionContainerRepository.UpdateAsync(entity, autoSave: true);
            }
            else
            {
                // create container case
                await _ExamQuestionContainerRepository.InsertAsync(
                    ObjectMapper.Map<ExamQuestionContainerDTO, ExamQuestionContainer>(inp),
                    autoSave: true);
            }
            // Sync question
            foreach (var question in inp.Questions)
            {
                if (await _ExamQuestionRepository.AnyAsync(x => x.Id == question.Id))
                {
                    // update container case
                    var q = await _ExamQuestionRepository.GetAsync(question.Id);
                    if (q == null)
                    {
                        res.Message = "Not found question";
                        return res;
                    }
                    //TODO: Check if input has id different than given id and normalize if it's default value, throw ex otherwise
                    ObjectMapper.Map(question, q);
                    // update question case
                    await _ExamQuestionRepository.UpdateAsync(q, autoSave: true);
                }
                else
                {
                    // create question case
                    await _ExamQuestionRepository.InsertAsync(
                        ObjectMapper.Map<ExamQuestionDTO, ExamQuestion>(question),
                        autoSave: true);
                }
                // Sync answers
                foreach (var answers in question.Answers)
                {
                    if (await _ExamAnswerRepository.AnyAsync(x => x.Id == answers.Id))
                    {
                        // update container case
                        var a = await _ExamAnswerRepository.GetAsync(answers.Id);
                        if (a == null)
                        {
                            res.Message = "Not found answer";
                            return res;
                        }
                        //TODO: Check if input has id different than given id and normalize if it's default value, throw ex otherwise
                        ObjectMapper.Map(answers, a);
                        // update question case
                        await _ExamAnswerRepository.UpdateAsync(a, autoSave: true);
                    }
                    else
                    {
                        // create question case
                        await _ExamAnswerRepository.InsertAsync(
                            ObjectMapper.Map<ExamAnswerDTO, ExamAnswer>(answers),
                            autoSave: true);
                    }
                }
            }

            return await GetForUpdateAsync(inp.Id);
        }
    }
}
