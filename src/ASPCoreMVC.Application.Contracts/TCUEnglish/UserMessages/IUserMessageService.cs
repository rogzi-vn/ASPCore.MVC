using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ASPCoreMVC.TCUEnglish.UserMessages
{
    public interface IUserMessageService : IWrapperCrudAppService<
        UserMessageDTO,
        Guid,
        GetUserMessageDTO>
    {
        public Task<ResponseWrapper<List<UserMessageDTO>>> GetPreviousMessages(Guid messGroupId, Guid latestMessId, int maxCount);
    }
}
