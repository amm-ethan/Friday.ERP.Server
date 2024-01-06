using Friday.ERP.Core.Data.Entities.AccountManagement;

namespace Friday.ERP.Core.IRepositories.Entities.AccountManagement;

public interface IUserRoleRepository
{
    Task<UserRole?> GetUserRoleByGuid(Guid guid, bool trackChanges);

    Task<IEnumerable<UserRole>> GetAllUserRoles(string? searchTerm, bool trackChanges);
}