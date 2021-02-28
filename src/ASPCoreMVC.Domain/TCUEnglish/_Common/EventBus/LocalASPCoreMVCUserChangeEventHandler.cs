using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace ASPCoreMVC.TCUEnglish._Common.EventBus
{
    // https://docs.abp.io/en/abp/latest/Customizing-Application-Modules-Extending-Entities
    public class LocalASPCoreMVCUserChangeEventHandler :
    ILocalEventHandler<EntityCreatedEventData<IdentityUser>>,
    ILocalEventHandler<EntityDeletedEventData<IdentityUser>>,
    ITransientDependency
    {
        public LocalASPCoreMVCUserChangeEventHandler()
        {
        }
        public async Task HandleEventAsync(EntityCreatedEventData<IdentityUser> eventData)
        {
        }

        public async Task HandleEventAsync(EntityDeletedEventData<IdentityUser> eventData)
        {
        }
    }
}
