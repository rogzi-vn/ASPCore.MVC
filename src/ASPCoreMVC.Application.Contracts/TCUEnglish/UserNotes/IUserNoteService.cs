using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.UserNotes
{
    public interface IUserNoteService : IWrapperCrudAppService<
        UserNoteDTO,
        Guid,
        GetUserNoteDTO>
    {
        public Task<ResponseWrapper<PagedResultDto<UserNoteBaseDTO>>> GetBaseListAsync(GetUserNoteDTO input);
    }
}
