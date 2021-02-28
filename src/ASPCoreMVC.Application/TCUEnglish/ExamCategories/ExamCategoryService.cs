using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
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
        public ExamCategoryService(IRepository<ExamCategory, Guid> repo) : base(repo)
        {

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
    }
}
