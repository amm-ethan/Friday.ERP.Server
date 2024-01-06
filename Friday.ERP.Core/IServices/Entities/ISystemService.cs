using Friday.ERP.Shared.DataTransferObjects;

namespace Friday.ERP.Core.IServices.Entities;

public interface ISystemService
{
    Task<SettingViewDto> GetSettings(string? wwwroot);

    Task<SettingViewDto> UpdateSettings(SettingUpdateDto settingUpdateDto, string wwwroot, string currentUserGuid);

    Task<Guid> CreateNotification(NotificationCreateDto notificationCreate, bool isSystemWide,
        List<Guid> sentUsers);

    Task<List<NotificationViewDto>> GetAllNotificationsByCurrentUserGuid(Guid userGuid);

    Task<NotificationViewDto> GetNotificationByGuid(Guid guid, Guid userGuid);

    Task ReadNotificationByGuidAndCurrentUserGuid(Guid guid, Guid userGuid);
}