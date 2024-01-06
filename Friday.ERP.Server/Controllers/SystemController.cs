using Friday.ERP.Core.IServices;
using Friday.ERP.Server.ActionFilters;
using Friday.ERP.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Friday.ERP.Server.Controllers;

[ApiController]
[Route("api/system-management")]
[Authorize]
public class SystemController(IServiceManager service, IWebHostEnvironment env) : ControllerBase
{
    private readonly string _wwwroot = env.WebRootPath;

    [HttpGet("system", Name = "GetSettings")]
    public async Task<IActionResult> GetSettings()
    {
        var settingToReturn = await service.SystemService.GetSettings(_wwwroot);
        return Ok(settingToReturn);
    }

    [HttpPut("system", Name = "UpdateSettings")]
    [ServiceFilter(typeof(GetCurrentUserGuidActionFilter))]
    public async Task<IActionResult> UpdateSettings(SettingUpdateDto settingUpdateDto)
    {
        var userGuid = HttpContext.Items["current_user_id"] as string;
        var settingToReturn = await service.SystemService.UpdateSettings(settingUpdateDto, _wwwroot, userGuid!);
        return Ok(settingToReturn);
    }

    [HttpGet("notifications", Name = "GetAllNotifications")]
    [ServiceFilter(typeof(GetCurrentUserGuidActionFilter))]
    public async Task<IActionResult> GetAllNotifications()
    {
        var userGuid = HttpContext.Items["current_user_id"] as string;
        var notificationsToReturn =
            await service.SystemService.GetAllNotificationsByCurrentUserGuid(Guid.Parse(userGuid!));
        return Ok(notificationsToReturn);
    }

    [HttpPost("notifications/{guid:guid}", Name = "ReadNotificationByGuid")]
    [ServiceFilter(typeof(GetCurrentUserGuidActionFilter))]
    public async Task<IActionResult> ReadNotificationByGuid(Guid guid)
    {
        var userGuid = HttpContext.Items["current_user_id"] as string;
        await service.SystemService.ReadNotificationByGuidAndCurrentUserGuid(guid, Guid.Parse(userGuid!));
        return Ok();
    }
}