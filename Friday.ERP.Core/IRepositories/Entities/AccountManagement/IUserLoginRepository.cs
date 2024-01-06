using Friday.ERP.Core.Data.Entities.AccountManagement;

namespace Friday.ERP.Core.IRepositories.Entities.AccountManagement;

public interface IUserLoginRepository
{
    void CreateUserLogin(UserLogin userLogin);

    void DeleteUserLogin(UserLogin userLogin);

    Task<UserLogin?> GetUserLoginByUserGuid(Guid userGuid, bool trackChanges);

    Task<UserLogin?> GetUserLoginByRefreshToken(string refreshToken, bool trackChanges);
}