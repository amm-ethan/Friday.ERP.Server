using Friday.ERP.Core.Data.Entities.SystemManagement;
using Friday.ERP.Core.Exceptions.NotFound;
using Friday.ERP.Core.IRepositories;
using Friday.ERP.Core.IServices;
using Friday.ERP.Core.IServices.Entities;
using Friday.ERP.Shared.DataTransferObjects;

namespace Friday.ERP.Infrastructure.Services.Entities;

internal sealed class SystemService(IRepositoryManager repository, ILoggerManager logger) : ISystemService
{
    public async Task<SettingViewDto> GetSettings(string? wwwroot)
    {
        var setting = await repository.Setting.GetSetting(false);
        if (setting is null)
            throw new ObjectNotFoundByFilterException("", "Setting",
                "");
        if (wwwroot is null) return ToSettingViewDto(setting, null);
        var imagePath = Path.Combine(wwwroot, "logo.png");
        var imageBytes = await File.ReadAllBytesAsync(imagePath);
        var base64Image = Convert.ToBase64String(imageBytes);
        return ToSettingViewDto(setting, base64Image);
    }

    public async Task<SettingViewDto> UpdateSettings(SettingUpdateDto settingUpdateDto, string wwwroot,
        string currentUserGuid)
    {
        var setting = await repository.Setting.GetSetting(true);
        if (setting is null)
            throw new ObjectNotFoundByFilterException("", "Setting", "");

        if (settingUpdateDto.DefaultProfitPercent is not null)
            setting!.DefaultProfitPercent = settingUpdateDto.DefaultProfitPercent ?? 10;
        if (settingUpdateDto.DefaultProfitPercentForWholeSale is not null)
            setting!.DefaultProfitPercentForWholeSale = settingUpdateDto.DefaultProfitPercentForWholeSale ?? 10;
        if (settingUpdateDto.MinimumStockMargin is not null)
            setting!.MinimumStockMargin = settingUpdateDto.MinimumStockMargin ?? 10;
        if (settingUpdateDto.SuggestSalePrice is not null)
            setting!.SuggestSalePrice = settingUpdateDto.SuggestSalePrice ?? false;

        await repository.SaveAsync();

        logger.LogInfo($"Setting is Updated by UserId {currentUserGuid}");
        return await GetSettings(wwwroot);
    }

    public async Task<Guid> CreateNotification(NotificationCreateDto notificationCreate, bool isSystemWide,
        List<Guid> sentUsers)
    {
        var notificationToCreate = Notification.FromNotificationCreateDto(notificationCreate, isSystemWide);
        if (isSystemWide)
            sentUsers.AddRange(await repository.User.GetAllUserGuids(false));

        foreach (var notificationUser in sentUsers.Select(userGuid => new NotificationUser
                 {
                     Guid = Guid.NewGuid(),
                     UserGuid = userGuid,
                     NotificationGuid = notificationToCreate.Guid,
                     HaveRead = false
                 }))
            repository.NotificationUser.CreateNotificationUser(notificationUser);
        repository.Notification.CreateNotification(notificationToCreate);
        await repository.SaveAsync();
        return notificationToCreate.Guid;
    }

    public async Task<List<NotificationViewDto>> GetAllNotificationsByCurrentUserGuid(Guid userGuid)
    {
        var notifications = await repository.Notification.GetAllNotificationsByUserGuid(userGuid, false);
        return notifications.Select(notification =>
            ToNotificationViewDto(notification, notification.NotificationUsers!.FirstOrDefault()!)).ToList();
    }

    public async Task<NotificationViewDto> GetNotificationByGuid(Guid guid, Guid userGuid)
    {
        var notification = await repository.Notification.GetNotificationByGuid(guid, false);
        if (notification is null)
            throw new ObjectNotFoundByFilterException("notificationGuid", "Notification",
                guid.ToString());

        var notificationUser =
            await repository.NotificationUser.GetNotificationUserByNotificationGuidAndUserGuid(guid, userGuid, false);
        if (notificationUser is null)
            throw new ObjectNotFoundByFilterException("notificationGuid, userGuid", "NotificationUser",
                guid.ToString());

        return ToNotificationViewDto(notification, notificationUser);
    }

    public async Task ReadNotificationByGuidAndCurrentUserGuid(Guid guid, Guid userGuid)
    {
        var notificationUserToUpdate =
            await repository.NotificationUser.GetNotificationUserByNotificationGuidAndUserGuid(guid, userGuid, true);
        if (notificationUserToUpdate is not null)
            throw new ObjectNotFoundByFilterException("", "NotificationUser",
                "");

        notificationUserToUpdate!.HaveRead = true;

        await repository.SaveAsync();
    }

    #region Private Methods

    private static SettingViewDto ToSettingViewDto(
        Setting setting, string? image)
    {
        return new SettingViewDto(
            image,
            setting.Name!,
            setting.AddressOne,
            setting.AddressTwo,
            setting.PhoneOne,
            setting.PhoneTwo,
            setting.PhoneThree,
            setting.PhoneFour,
            setting.DefaultProfitPercent,
            setting.DefaultProfitPercentForWholeSale,
            setting.MinimumStockMargin,
            setting.SuggestSalePrice
        );
    }

    private static NotificationViewDto ToNotificationViewDto(
        Notification notification, NotificationUser notificationUser)
    {
        return new NotificationViewDto
        (
            notification.Guid,
            notification.Heading!,
            notification.Body!,
            notificationUser.HaveRead ?? false,
            notification.SentAt
        );
    }

    #endregion
}