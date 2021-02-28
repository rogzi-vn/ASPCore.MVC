using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ASPCoreMVC._Commons.Services
{
    public interface IWrapperUpdateAppService<TEntityDto, in TKey>
        : IUpdateAppService<TEntityDto, TKey, TEntityDto>
    {

    }

    public interface IWrapperUpdateAppService<TGetOutputDto, in TKey, in TUpdateInput>
        : IApplicationService
    {
        Task<ResponseWrapper<TGetOutputDto>> UpdateAsync(TKey id, TUpdateInput input);
    }
}
