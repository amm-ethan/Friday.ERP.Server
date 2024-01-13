using Friday.ERP.Core.Data.Entities.AccountManagement;
using Friday.ERP.Core.IRepositories.Entities.AccountManagement;
using Microsoft.EntityFrameworkCore;

namespace Friday.ERP.Infrastructure.Repositories.Entities.AccountManagement;

internal sealed class UserRoleRepository(RepositoryContext repositoryContext)
    : RepositoryBase<UserRole>(repositoryContext), IUserRoleRepository
{
    public async Task<UserRole?> GetUserRoleByGuid(Guid guid, bool trackChanges)
    {
        return await FindByCondition(c => c.Guid.Equals(guid), trackChanges).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<UserRole>> GetAllUserRoles(string? searchTerm, bool trackChanges)
    {
        if (searchTerm is null)
            return await FindAll(trackChanges)
                .ToListAsync();

        var lowerCaseTerm = searchTerm.Trim().ToLower();
        return await FindByCondition(c => c.Name!.ToLower().Contains(lowerCaseTerm)
                , trackChanges)
            .ToListAsync();
    }
}