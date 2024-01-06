using Friday.ERP.Core.IServices;
using Friday.ERP.Server.ActionFilters;
using Friday.ERP.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Friday.ERP.Server.Controllers;

[ApiController]
[Route("api/authentication")]
[AllowAnonymous]
public class AuthenticationController(IServiceManager service) : ControllerBase
{
    [HttpPost("login", Name = "Login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var user = await service.AuthenticationService.ValidateLoginInfo(loginDto);
        var dataToReturn = await service.AuthenticationService.GenerateLoginTokens(user.Guid);
        return Ok(dataToReturn);
    }

    [HttpPost("logout", Name = "logout")]
    [ServiceFilter(typeof(GetCurrentUserGuidActionFilter))]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var userGuid = HttpContext.Items["current_user_id"] as string;
        await service.AuthenticationService.Logout(Guid.Parse(userGuid!));
        return Ok();
    }

    [HttpPost("refresh", Name = "refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenDto tokenRequestDto)
    {
        var dataToReturn = await service.AuthenticationService.GenerateAccessTokenFromRefreshToken(tokenRequestDto);
        return Ok(dataToReturn);
    }
}