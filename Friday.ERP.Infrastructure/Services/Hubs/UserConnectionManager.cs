using Friday.ERP.Core.IServices.Hubs;

namespace Friday.ERP.Infrastructure.Services.Hubs;

public sealed class UserConnectionManager : IUserConnectionManager
{
    private static readonly Dictionary<string, List<string>> UserConnectionMap = new();
    private static readonly string UserConnectionMapLocker = string.Empty;

    public void KeepUserConnection(string userId, string connectionId)
    {
        lock (UserConnectionMapLocker)
        {
            if (!UserConnectionMap.ContainsKey(userId)) UserConnectionMap[userId] = new List<string>();
            UserConnectionMap[userId].Add(connectionId);
        }
    }

    public void RemoveUserConnection(string connectionId)
    {
        lock (UserConnectionMapLocker)
        {
            foreach (var userId in UserConnectionMap.Keys.Where(userId => UserConnectionMap.ContainsKey(userId))
                         .Where(userId => UserConnectionMap[userId].Contains(connectionId)))
            {
                UserConnectionMap[userId].Remove(connectionId);
                break;
            }
        }
    }

    public List<string> GetUserConnections(string userId)
    {
        List<string> connections;
        lock (UserConnectionMapLocker)
        {
            connections = UserConnectionMap[userId];
        }

        return connections;
    }

    public List<string> GetAllActiveUserConnections()
    {
        var connections = new List<string>();
        lock (UserConnectionMapLocker)
        {
            foreach (var key in UserConnectionMap) connections.AddRange(key.Value);
        }

        return connections;
    }
}