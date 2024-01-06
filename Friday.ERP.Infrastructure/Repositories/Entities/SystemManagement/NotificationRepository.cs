using Friday.ERP.Core.Data.Entities.SystemManagement;
using Friday.ERP.Core.IRepositories.Entities.SystemManagement;
using Microsoft.EntityFrameworkCore;

namespace Friday.ERP.Infrastructure.Repositories.Entities.SystemManagement;

internal sealed class NotificationRepository(RepositoryContext repositoryContext)
    : RepositoryBase<Notification>(repositoryContext), INotificationRepository
{
    public void CreateNotification(Notification notification)
    {
        Create(notification);
    }

    public async Task<IEnumerable<Notification>> GetAllNotificationsByUserGuid(Guid userGuid, bool trackChanges)
    {
        return await FindAll(trackChanges)
            .Include(c => c.NotificationUsers!.Where(x => x.UserGuid == userGuid))
            .OrderByDescending(c => c.SentAt)
            .Take(10)
            .ToListAsync();
    }

    public async Task<Notification?> GetNotificationByGuid(Guid guid, bool trackChanges)
    {
        return await FindByCondition(c => c.Guid == guid, trackChanges).Include(c => c.NotificationUsers)
            .SingleOrDefaultAsync();
    }
}