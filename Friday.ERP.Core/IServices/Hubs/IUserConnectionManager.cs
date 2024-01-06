namespace Friday.ERP.Core.IServices.Hubs;

public interface IUserConnectionManager
{
    void KeepUserConnection(string userId, string connectionId);

    void RemoveUserConnection(string connectionId);

    List<string> GetUserConnections(string userId);

    List<string> GetAllActiveUserConnections();
}