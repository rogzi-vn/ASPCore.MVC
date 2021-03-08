using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.SkillParts
{
    public interface ISkillPartService : IWrapperCrudAppService<
        SkillPartDTO,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateSkillPartDTO>
    {
        public ResponseWrapper<List<SkillPartBaseDTO>> GetBase(Guid skillCatId);
        public ResponseWrapper<SkillPartBaseDTO> GetSimpify(Guid id);
        public Task<ResponseWrapper<bool>> GetHasAsync(Guid skillPartId);
        public Task PutUpdateOrder(List<Guid> skillPartIds);
        public Task<ResponseWrapper<CreateUpdateSkillPartDTO>> GetDataForUpdate(Guid skillPartId);
    }
}
