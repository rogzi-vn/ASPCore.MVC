using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPCoreMVC.TCUEnglish.MessGroups
{
    public interface IMessGroupService : IWrapperCrudAppService<
        MessGroupDTO,
        Guid,
        GetMessGroupDTO>
    {
    }
}
