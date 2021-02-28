using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ASPCoreMVC._Commons.Services
{
    interface IWrapperReadOnlyAppService<TEntityDto, in TKey>
        : IWrapperReadOnlyAppService<TEntityDto, TEntityDto, TKey, PagedAndSortedResultRequestDto>
    {

    }

    public interface IWrapperReadOnlyAppService<TEntityDto, in TKey, in TGetListInput>
        : IWrapperReadOnlyAppService<TEntityDto, TEntityDto, TKey, TGetListInput>
    {

    }

    public interface IWrapperReadOnlyAppService<TGetOutputDto, TGetListOutputDto, in TKey, in TGetListInput>
        : IApplicationService
    {
        Task<ResponseWrapper<TGetOutputDto>> GetAsync(TKey id);

        Task<ResponseWrapper<PagedResultDto<TGetListOutputDto>>> GetListAsync(TGetListInput input);
    }
}
