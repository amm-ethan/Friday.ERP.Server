using Friday.ERP.Core.Data.Entities.AccountManagement;
using Friday.ERP.Shared.DataTransferObjects;

namespace Friday.ERP.Core.IServices.Entities;

public interface IAuthenticationService
{
    Task<User> ValidateLoginInfo(LoginDto loginDto);

    Task<LoginResultDto> GenerateLoginTokens(Guid guid);

    Task<LoginResultDto> GenerateAccessTokenFromRefreshToken(RefreshTokenDto refreshTokenDto);

    Task Logout(Guid userGuid);
}