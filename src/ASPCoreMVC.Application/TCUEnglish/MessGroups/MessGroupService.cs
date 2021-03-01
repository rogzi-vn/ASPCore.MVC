using ASPCoreMVC._Commons.Services;
using ASPCoreMVC.TCUEnglish.MesGroups;
using System;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.TCUEnglish.MessGroups
{
    public class MessGroupService : WrapperCrudAppService<
        MessGroup,
        MessGroupDTO,
        Guid,
        GetMessGroupDTO>, IMessGroupService
    {
        public MessGroupService(IRepository<MessGroup, Guid> repo) : base(repo) { }
    }
}
