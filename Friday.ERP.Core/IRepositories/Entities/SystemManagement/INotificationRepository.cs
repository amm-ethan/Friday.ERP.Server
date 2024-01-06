using Friday.ERP.Core.Data.Entities.SystemManagement;

namespace Friday.ERP.Core.IRepositories.Entities.SystemManagement;

public interface INotificationRepository
{
    void CreateNotification(Notification notification);
    Task<Notification?> GetNotificationByGuid(Guid guid, bool trackChanges);
    Task<IEnumerable<Notification>> GetAllNotificationsByUserGuid(Guid userGuid, bool trackChanges);
}