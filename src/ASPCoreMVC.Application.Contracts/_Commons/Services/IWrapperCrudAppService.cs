using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ASPCoreMVC._Commons.Services
{
    public interface IWrapperCrudAppService<TEntityDto, in TKey>
        : IWrapperCrudAppService<TEntityDto, TKey, PagedAndSortedResultRequestDto>
    {

    }

    public interface IWrapperCrudAppService<TEntityDto, in TKey, in TGetListInput>
        : IWrapperCrudAppService<TEntityDto, TKey, TGetListInput, TEntityDto>
    {

    }

    public interface IWrapperCrudAppService<TEntityDto, in TKey, in TGetListInput, in TCreateInput>
        : IWrapperCrudAppService<TEntityDto, TKey, TGetListInput, TCreateInput, TCreateInput>
    {

    }

    public interface IWrapperCrudAppService<TEntityDto, in TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
        : IWrapperCrudAppService<TEntityDto, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
    {

    }

    public interface IWrapperCrudAppService<TGetOutputDto, TGetListOutputDto, in TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
        : IWrapperReadOnlyAppService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput>,
            IWrapperCreateUpdateAppService<TGetOutputDto, TKey, TCreateInput, TUpdateInput>,
            IDeleteAppService<TKey>
    {

    }
}
