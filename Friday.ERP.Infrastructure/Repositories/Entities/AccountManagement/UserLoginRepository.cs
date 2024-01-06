using Friday.ERP.Core.Data.Entities.AccountManagement;
using Friday.ERP.Core.IRepositories.Entities.AccountManagement;
using Microsoft.EntityFrameworkCore;

namespace Friday.ERP.Infrastructure.Repositories.Entities.AccountManagement;

internal sealed class UserLoginRepository(RepositoryContext repositoryContext)
    : RepositoryBase<UserLogin>(repositoryContext), IUserLoginRepository
{
    public async Task<UserLogin?> GetUserLoginByUserGuid(Guid userGuid, bool trackChanges)
    {
        return await FindByCondition(c => c.UserGuid == userGuid, trackChanges)
            .SingleOrDefaultAsync();
    }

    public async Task<UserLogin?> GetUserLoginByRefreshToken(string refreshToken, bool trackChanges)
    {
        return await FindByCondition(c => c.RefreshToken == refreshToken, trackChanges).SingleOrDefaultAsync();
    }

    public void CreateUserLogin(UserLogin userLogin)
    {
        Create(userLogin);
    }

    public void DeleteUserLogin(UserLogin userLogin)
    {
        Delete(userLogin);
    }
}