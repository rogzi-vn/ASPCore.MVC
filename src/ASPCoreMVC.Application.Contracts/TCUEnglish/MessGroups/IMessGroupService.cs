using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ASPCoreMVC.TCUEnglish.MessGroups
{
    public interface IMessGroupService : ICrudAppService<
        MessGroupDTO,
        Guid,
        GetMessGroupDTO>
    {
    }
}
