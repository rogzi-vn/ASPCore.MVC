using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.TCUEnglish.UserMessages
{
    public class UserMessageService : WrapperCrudAppService<
        UserMessage,
        UserMessageDTO,
        Guid,
        GetUserMessageDTO>, IUserMessageService
    {
        public UserMessageService(IRepository<UserMessage, Guid> repo) : base(repo) { }
        public async Task<ResponseWrapper<List<UserMessageDTO>>> GetPreviousMessages(Guid messGroupId, Guid latestMessId, int maxCount)
        {
            if (messGroupId == Guid.Empty)
            {
                return new ResponseWrapper<List<UserMessageDTO>>()
                    .ErrorReponseWrapper(new List<UserMessageDTO>(), "Please tell what group message", 404);
            }
            var currentUserMessage = await Repository.GetAsync(latestMessId);
            if (currentUserMessage != null)
            {
                var query = await Repository.GetQueryableAsync();
                query = query.Where(x => x.MessGroupId == messGroupId);
                query = query.OrderByDescending(x => x.CreationTime);
                query = query.Where(x => x.CreationTime < currentUserMessage.CreationTime)
                    .Take(maxCount);
                return new ResponseWrapper<List<UserMessageDTO>>()
                    .SuccessReponseWrapper(ObjectMapper.Map<List<UserMessage>,
                    List<UserMessageDTO>>(query.ToList()), "Successfull");
            }
            else
            {
                return new ResponseWrapper<List<UserMessageDTO>>()
                    .ErrorReponseWrapper(new List<UserMessageDTO>(), "Not found current message", 404);
            }
        }
    }
}
