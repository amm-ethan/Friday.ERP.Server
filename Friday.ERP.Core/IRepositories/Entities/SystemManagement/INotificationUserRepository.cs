using Friday.ERP.Core.Data.Entities.SystemManagement;

namespace Friday.ERP.Core.IRepositories.Entities.SystemManagement;

public interface INotificationUserRepository
{
    void CreateNotificationUser(NotificationUser notificationUser);

    Task<NotificationUser?> GetNotificationUserByNotificationGuidAndUserGuid(Guid notificationGuid, Guid userGuid,
        bool trackChanges);
}