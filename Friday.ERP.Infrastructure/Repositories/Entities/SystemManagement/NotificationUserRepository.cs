using Friday.ERP.Core.Data.Entities.SystemManagement;
using Friday.ERP.Core.IRepositories.Entities.SystemManagement;
using Microsoft.EntityFrameworkCore;

namespace Friday.ERP.Infrastructure.Repositories.Entities.SystemManagement;

internal sealed class NotificationUserRepository(RepositoryContext repositoryContext)
    : RepositoryBase<NotificationUser>(repositoryContext), INotificationUserRepository
{
    public void CreateNotificationUser(NotificationUser notificationUser)
    {
        Create(notificationUser);
    }

    public async Task<NotificationUser?> GetNotificationUserByNotificationGuidAndUserGuid(Guid notificationGuid,
        Guid userGuid, bool trackChanges)
    {
        return await FindByCondition(c => c.NotificationGuid == notificationGuid && c.UserGuid == userGuid,
            trackChanges).SingleOrDefaultAsync();
    }
}