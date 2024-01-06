using Friday.ERP.Core.IServices.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Friday.ERP.Server.Hubs;

public class NotificationHub(IUserConnectionManager userConnectionManager) : Hub
{
    public string GetConnectionId()
    {
        var httpContext = Context.GetHttpContext();
        var userId = httpContext!.Request.Query["userId"];

        userConnectionManager.KeepUserConnection(userId!, Context.ConnectionId);
        return Context.ConnectionId;
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;
        userConnectionManager.RemoveUserConnection(connectionId);
        await base.OnDisconnectedAsync(exception);
    }
}