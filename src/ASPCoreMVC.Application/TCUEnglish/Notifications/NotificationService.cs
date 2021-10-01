using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.TCUEnglish.Notifications
{
    public class NotificationService : CrudAppService<Notification, NotificationDTO, Guid>, INotificationService
    {
        public NotificationService(IRepository<Notification, Guid> repo) : base(repo)
        {
        }

        public async Task UpdateNotificationSeenState(Guid id)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => x.TargetUserId == CurrentUser.Id);
            var currentNotification = query.FirstOrDefault(x => x.Id == id);
            if (currentNotification != null)
            {
                currentNotification.IsChecked = true;
                await Repository.UpdateAsync(currentNotification, true);
            }
        }

        protected override async Task<IQueryable<Notification>> CreateFilteredQueryAsync(
            PagedAndSortedResultRequestDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => CurrentUser.Id == null || x.TargetUserId.Value == CurrentUser.Id.Value);
            return query;
        }

        protected override IQueryable<Notification> ApplyDefaultSorting(IQueryable<Notification> query)
        {
            return query
                .OrderByDescending(x => x.CreationTime)
                .ThenBy(x => x.IsChecked);
        }

        public async Task<int> GetCountUnreadNotification()
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => CurrentUser.Id == null || x.TargetUserId.Value == CurrentUser.Id.Value);
            query = query.Where(x => !x.IsChecked);
            return query.Count();
        }

        public async Task MarkAllAsRead()
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => CurrentUser.Id == null || x.TargetUserId.Value == CurrentUser.Id.Value);
            query = query.Where(x => !x.IsChecked);
            var unread = query.ToList();
            unread.ForEach(notify => { notify.IsChecked = true; });
            await Repository.UpdateManyAsync(unread);
        }
    }
}