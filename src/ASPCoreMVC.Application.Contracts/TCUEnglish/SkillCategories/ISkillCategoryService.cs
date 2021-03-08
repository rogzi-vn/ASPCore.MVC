using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.SkillCategories
{
    public interface ISkillCategoryService : IWrapperCrudAppService<
        SkillCategoryDTO,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateSkillCategoryDTO>
    {
        public ResponseWrapper<List<SkillCategoryBaseDTO>> GetBase(Guid exCatId);
        public ResponseWrapper<SkillCategoryBaseDTO> GetSimpify(Guid id);
        public Task<ResponseWrapper<bool>> GetHasAsync(Guid skillCatId);
        public Task PutUpdateOrder(List<Guid> skillCatIds);
        public Task<ResponseWrapper<CreateUpdateSkillCategoryDTO>> GetDataForUpdate(Guid skillCatId);
    }
}
