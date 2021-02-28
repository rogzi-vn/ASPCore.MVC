using System;
using System.Collections.Generic;
using System.Text;

namespace ASPCoreMVC._Commons.Services
{
    public interface IWrapperCreateUpdateAppService<TEntityDto, in TKey>
        : IWrapperCreateUpdateAppService<TEntityDto, TKey, TEntityDto, TEntityDto>
    {

    }

    public interface IWrapperCreateUpdateAppService<TEntityDto, in TKey, in TCreateUpdateInput>
        : IWrapperCreateUpdateAppService<TEntityDto, TKey, TCreateUpdateInput, TCreateUpdateInput>
    {

    }

    public interface IWrapperCreateUpdateAppService<TGetOutputDto, in TKey, in TCreateUpdateInput, in TUpdateInput>
        : IWrapperCreateAppService<TGetOutputDto, TCreateUpdateInput>,
            IWrapperUpdateAppService<TGetOutputDto, TKey, TUpdateInput>
    {

    }
}
