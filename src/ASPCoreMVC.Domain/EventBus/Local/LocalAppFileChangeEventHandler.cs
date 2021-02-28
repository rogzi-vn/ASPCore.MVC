using ASPCoreMVC.Helpers;
using ASPCoreMVC.TCUEnglish.AppFiles;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;

namespace ASPCoreMVC.EventBus.Local
{
    public class LocalAppFileChangeEventHandler :
        ILocalEventHandler<EntityCreatedEventData<AppFile>>
    {
        private readonly IRepository<AppFile, Guid> _AppFileRepository;

        public LocalAppFileChangeEventHandler(
            IRepository<AppFile, Guid> AppFileRepository)
        {
            _AppFileRepository = AppFileRepository;
        }
        public async Task HandleEventAsync(EntityCreatedEventData<AppFile> eventData)
        {
            var id = eventData.Entity.Id;
            if (id != AppFileDefaults.RootDirectory.Id && eventData.Entity.ParentId == null)
            {
                var cloneAppFile = new AppFile
                {
                    ParentId = AppFileDefaults.RootDirectory.Id,
                    Name = eventData.Entity.Name,
                    IsDirectory = eventData.Entity.IsDirectory,
                    Length = eventData.Entity.Length
                }.SetId(id)
                .SetPath(
                    PathHelper.TrueCombine(AppFileDefaults.RootDirectory.Path,
                    eventData.Entity.Name));
                await _AppFileRepository.UpdateAsync(cloneAppFile);
            }
            else if (eventData.Entity.Path.IsNullOrEmpty())
            {
                var parent = await _AppFileRepository.GetAsync(eventData.Entity.ParentId.Value);
                var cloneAppFile = new AppFile
                {
                    ParentId = parent.Id,
                    Name = eventData.Entity.Name,
                    IsDirectory = eventData.Entity.IsDirectory,
                    Length = eventData.Entity.Length
                }.SetId(id)
                .SetPath(
                    PathHelper.TrueCombine(parent.Path,
                    eventData.Entity.Name));
                await _AppFileRepository.UpdateAsync(cloneAppFile);
            }
        }
    }
}
