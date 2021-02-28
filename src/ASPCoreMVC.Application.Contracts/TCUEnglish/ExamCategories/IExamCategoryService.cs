using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.ExamCategories
{
    public interface IExamCategoryService :
        IWrapperCrudAppService<
            ExamCategoryDTO,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateExamCategoryDTO>
    {
        public Task<ResponseWrapper<List<ExamCategoryBaseDTO>>> GetBase();
        public Task<ResponseWrapper<ExamCategoryBaseDTO>> GetSimpify(Guid guid);
        public Task<ResponseWrapper<CreateUpdateExamCategoryDTO>> GetForUpdate(Guid guid);
        public Task<ResponseWrapper<bool>> GetHasAsync(Guid id);
    }
}
