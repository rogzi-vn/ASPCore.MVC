using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ASPCoreMVC._Commons.Services
{
    public interface IWrapperCreateAppService<TEntityDto>
        : ICreateAppService<TEntityDto, TEntityDto>
    {

    }

    public interface IWrapperCreateAppService<TGetOutputDto, in TCreateInput>
        : IApplicationService
    {
        Task<ResponseWrapper<TGetOutputDto>> CreateAsync(TCreateInput input);
    }
}
