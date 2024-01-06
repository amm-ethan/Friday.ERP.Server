using Friday.ERP.Core.IServices;
using Friday.ERP.Server.ActionFilters;
using Friday.ERP.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Friday.ERP.Server.Controllers;

[ApiController]
[Route("api/account-management")]
[Authorize]
public class AccountController(IServiceManager service) : ControllerBase
{
    [HttpPost("users", Name = "CreateUser")]
    public async Task<IActionResult> CreateUser(UserCreateDto userCreateDto)
    {
        var userToReturn = await service.AccountService.CreateUser(userCreateDto);

        return Ok(userToReturn);
    }

    [HttpGet("users", Name = "GetAllUsers")]
    public async Task<IActionResult> GetAllUsers([FromQuery] UserParameter userParameter)
    {
        var (usersToReturn, metaData) = await service.AccountService.GetAllUsers(userParameter);
        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(metaData);
        return Ok(usersToReturn);
    }

    [HttpGet("users/{guid:guid}", Name = "GetUserByGuid")]
    public async Task<IActionResult> GetUserByGuid(Guid guid)
    {
        var userToReturn = await service.AccountService.GetUserByGuid(guid);
        return Ok(userToReturn);
    }

    [HttpPut("users/{guid:guid}", Name = "UpdateUserByGuid")]
    [ServiceFilter(typeof(GetCurrentUserGuidActionFilter))]
    public async Task<IActionResult> UpdateUserByGuid(Guid guid, UserUpdateDto userUpdateDto)
    {
        var userGuid = HttpContext.Items["current_user_id"] as string;
        var userToReturn = await service.AccountService.UpdateUserByGuid(guid, userUpdateDto, userGuid!);
        return Ok(userToReturn);
    }

    [HttpGet("roles", Name = "GetAllUserRoles")]
    public async Task<IActionResult> GetAllUserRoles([FromQuery] string? searchTerm)
    {
        var userRolesToReturn = await service.AccountService.GetAllUserRoles(searchTerm);
        return Ok(userRolesToReturn);
    }
}