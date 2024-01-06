using Friday.ERP.Core.Exceptions.NotFound;
using Friday.ERP.Core.Exceptions.Unauthorized;
using Friday.ERP.Core.IRepositories;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Friday.ERP.Server.ActionFilters;

public class GetCurrentUserGuidActionFilter(IRepositoryManager repository) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var req = context.HttpContext.Request;
        var result = req.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id");
        if (result == null)
            throw new WrongRefreshTokenUnauthorizedException();

        var guid = Guid.Parse(result.Value);
        var user = await repository.User.GetUserByGuid(guid, false);
        if (user == null)
            throw new ObjectNotFoundByFilterException("User", "Guid", guid.ToString());

        context.HttpContext.Items.Add("current_user_id", user.Guid.ToString());
        await next();
    }
}