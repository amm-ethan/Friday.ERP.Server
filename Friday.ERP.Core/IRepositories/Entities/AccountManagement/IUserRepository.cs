using Friday.ERP.Core.Data.Entities.AccountManagement;
using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.RequestFeatures;

namespace Friday.ERP.Core.IRepositories.Entities.AccountManagement;

public interface IUserRepository
{
    void CreateUser(User user);

    Task<User?> GetUserByGuid(Guid guid, bool trackChanges);

    Task<User?> GetUserByUsername(string username, bool trackChanges);

    Task<User?> GetUserByName(string name, bool trackChanges);

    Task<User?> GetUserByPhone(string phone, bool trackChanges);

    Task<User?> GetUserByEmail(string email, bool trackChanges);

    Task<PagedList<User>> GetAllUsers(UserParameter userParameter, bool trackChanges);

    Task<IEnumerable<Guid>> GetAllUserGuids(bool trackChanges);
}