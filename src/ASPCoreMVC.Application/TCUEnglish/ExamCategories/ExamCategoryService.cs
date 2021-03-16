using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using ASPCoreMVC.TCUEnglish.ExamSkillCategories;
using ASPCoreMVC.TCUEnglish.ExamSkillParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.TCUEnglish.ExamCategories
{
    public class ExamCategoryService : WrapperCrudAppService<
        ExamCategory,
        ExamCategoryDTO,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateExamCategoryDTO>, IExamCategoryService
    {

        private readonly IRepository<ExamSkillCategory, Guid> ExamSkillCategoryRepory;
        private readonly IRepository<ExamSkillPart, Guid> ExamSkillPartRepository;

        public ExamCategoryService(IRepository<ExamCategory, Guid> repo,
            IRepository<ExamSkillCategory, Guid> ExamSkillCategoryRepory,
            IRepository<ExamSkillPart, Guid> ExamSkillPartRepository) : base(repo)
        {
            this.ExamSkillCategoryRepory = ExamSkillCategoryRepory;
            this.ExamSkillPartRepository = ExamSkillPartRepository;
        }

        protected override IQueryable<ExamCategory> ApplySorting(IQueryable<ExamCategory> query, PagedAndSortedResultRequestDto input)
        {
            return query.OrderBy(x => x.CreationTime);
        }

        public override Task DeleteAsync(Guid id)
        {
            return base.DeleteAsync(id);
        }

        public async Task<ResponseWrapper<List<ExamCategoryBaseDTO>>> GetBase()
        {
            return new ResponseWrapper<List<ExamCategoryBaseDTO>>(
                ObjectMapper.Map<List<ExamCategory>, List<ExamCategoryBaseDTO>>(
                await Repository.GetListAsync()),
                "Successful");
        }

        public async Task<ResponseWrapper<CreateUpdateExamCategoryDTO>> GetForUpdate(Guid guid)
        {
            return new ResponseWrapper<CreateUpdateExamCategoryDTO>(
               ObjectMapper.Map<ExamCategory, CreateUpdateExamCategoryDTO>(
               await Repository.GetAsync(guid)),
               "Successful");
        }

        public async Task<ResponseWrapper<bool>> GetHasAsync(Guid id)
        {
            return new ResponseWrapper<bool>(await Repository.AnyAsync(x => x.Id == id), "Successful");
        }

        public async Task<ResponseWrapper<ExamCategoryBaseDTO>> GetSimpify(Guid guid)
        {
            return new ResponseWrapper<ExamCategoryBaseDTO>(
                            ObjectMapper.Map<ExamCategory, ExamCategoryBaseDTO>(
                            await Repository.GetAsync(guid)),
                            "Successful");
        }

        public float GetMaxScores(Guid id)
        {
            return ExamSkillCategoryRepory
                .Where(x => x.ExamCategoryId == id)
                .Sum(x => x.MaxScores);
        }

        public async Task<List<SkillCatMinifyDTO>> GetFullChild(Guid id)
        {
            // Lấy danh sách các mục kỹ năng
            var scQuery = await ExamSkillCategoryRepory.GetQueryableAsync();
            var skillCats = scQuery
                .Where(x => x.ExamCategoryId == id)
                .OrderBy(x => x.Order)
                .Select(x => new SkillCatMinifyDTO
                {
                    Id = x.Id,
                    ExamCategoryId = x.ExamCategoryId,
                    Order = x.Order,
                    Name = x.Name
                }).ToList();
            // Ráp các phần thi thuộc các mục kỹ năng vào
            var spQuery = await ExamSkillPartRepository.GetQueryableAsync();
            for (int i = 0; i < skillCats.Count; i++)
            {
                var skps = spQuery
                    .Where(x => x.ExamSkillCategoryId == skillCats[i].Id)
                    .OrderBy(x => x.Order)
                    .Select(x => new SkillPartMinifyDTO
                    {
                        Id = x.Id,
                        ExamSkillCategoryId = x.ExamSkillCategoryId,
                        Order = x.Order,
                        Name = x.Name
                    }).ToList();
                skillCats[i].SkillParts = skps;
            }

            return skillCats;
        }
    }
}
