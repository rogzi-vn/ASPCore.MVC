using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ASPCoreMVC.TCUEnglish.UserMessages
{
    public interface IUserMessageService : ICrudAppService<
        UserMessageDTO,
        Guid,
        GetUserMessageDTO>
    {
        public Task<List<UserMessageDTO>> GetPreviousMessages(Guid messGroupId, int maxCount, Guid? oldestMsgId);
    }
}
