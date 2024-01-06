using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;
using Friday.ERP.Core.Data.Constants.AppSettings;
using Friday.ERP.Core.Data.Entities.AccountManagement;
using Friday.ERP.Core.Exceptions.NotFound;
using Friday.ERP.Core.Exceptions.Unauthorized;
using Friday.ERP.Core.IRepositories;
using Friday.ERP.Core.IServices;
using Friday.ERP.Core.IServices.Entities;
using Friday.ERP.Shared.DataTransferObjects;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Friday.ERP.Infrastructure.Services.Entities;

internal sealed class AuthenticationService(
    IRepositoryManager repository,
    ILoggerManager logger,
    IOptionsMonitor<JwtConfiguration> jwtConfiguration)
    : IAuthenticationService
{
    private readonly JwtConfiguration _jwtConfiguration = jwtConfiguration.CurrentValue;


    public async Task<User> ValidateLoginInfo(LoginDto loginDto)
    {
        var currentUser = await repository.User.GetUserByUsername(loginDto.Username, false);
        if (currentUser is null)
            throw new ObjectNotFoundByFilterException("Username", "User",
                loginDto.Username);

        var loginPassword = Encoding.UTF8.GetString(loginDto.Password);
        var isCorrect = BCrypt.Net.BCrypt.EnhancedVerify(loginPassword, currentUser.Password, HashType.SHA512);
        if (!isCorrect)
            throw new WrongPasswordUnauthorizedException();

        logger.LogInfo($"User {loginDto.Username} login!");
        return currentUser;
    }

    public async Task<LoginResultDto> GenerateLoginTokens(Guid guid)
    {
        var user = await repository.User.GetUserByGuid(guid, true);
        var accessToken = GenerateAccessToken(user!);
        var (refreshToken, refreshTokenExpiryTime) = GenerateRefreshTokenAndExpiryTime();

        var existingUserLogin =
            await repository.UserLogin.GetUserLoginByUserGuid(user!.Guid, true);
        if (existingUserLogin is null)
        {
            var currentUserLogin = new UserLogin
            {
                Guid = Guid.NewGuid(),
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenExpiryAt = refreshTokenExpiryTime,
                UserGuid = user.Guid
            };
            repository.UserLogin.CreateUserLogin(currentUserLogin);
        }
        else
        {
            existingUserLogin.AccessToken = accessToken;
            existingUserLogin.RefreshToken = refreshToken;
            existingUserLogin.RefreshTokenExpiryAt = refreshTokenExpiryTime;
        }

        await repository.SaveAsync();
        return new LoginResultDto
            (accessToken, refreshToken, refreshTokenExpiryTime);
    }

    public async Task<LoginResultDto> GenerateAccessTokenFromRefreshToken(
        RefreshTokenDto refreshTokenDto)
    {
        var principal = GetPrincipalFromExpiredToken(refreshTokenDto.AccessToken);
        var userGuid = principal.FindFirst("id")!.Value;

        var currentUser = await repository.User.GetUserByGuid(Guid.Parse(userGuid), false);
        var existingUserLogin =
            await repository.UserLogin.GetUserLoginByRefreshToken(refreshTokenDto.RefreshToken, true);

        if (currentUser is null || existingUserLogin is null || DateTime.Now >= existingUserLogin.RefreshTokenExpiryAt)
            throw new WrongRefreshTokenUnauthorizedException();

        var accessToken = GenerateAccessToken(currentUser);
        var (refreshToken, refreshTokenExpiryTime) = GenerateRefreshTokenAndExpiryTime();

        existingUserLogin.AccessToken = accessToken;
        existingUserLogin.RefreshToken = refreshToken;
        existingUserLogin.RefreshTokenExpiryAt = refreshTokenExpiryTime;

        await repository.SaveAsync();
        return new LoginResultDto
            (accessToken, refreshToken, refreshTokenExpiryTime);
    }

    public async Task Logout(Guid userGuid)
    {
        var userLogin = await repository.UserLogin.GetUserLoginByUserGuid(userGuid, true);
        if (userLogin is not null)
            repository.UserLogin.DeleteUserLogin(userLogin);

        await repository.SaveAsync();
        logger.LogInfo($"User {userGuid} logout!");
    }

    #region Private Methods

    private string GenerateAccessToken(User user)
    {
        var signingCredentials = new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);

        var claims = GetClaims(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return accessToken;
    }

    private Tuple<string, DateTime> GenerateRefreshTokenAndExpiryTime()
    {
        var refreshToken = CreateRefreshToken();
        var refreshTokenExpiryTime = DateTime.Now.AddDays(Convert.ToDouble(_jwtConfiguration.RefreshTokenExpires));

        return new Tuple<string, DateTime>(refreshToken, refreshTokenExpiryTime);
    }

    private SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        var key = Encoding.UTF8.GetBytes(_jwtConfiguration.JwtSecret!);
        return new SymmetricSecurityKey(key);
    }

    private IEnumerable<Claim> GetClaims(User user)
    {
        List<Claim> claim =
        [
            new Claim(JwtRegisteredClaimNames.Iss, "friday.ERP.Server"),
            new Claim("id", user.Guid.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Name!),
            new Claim("username", user.Username!),
            new Claim("email_address", user.Email!),
            new Claim("role", user.UserRole!.Name!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64),

            new Claim(JwtRegisteredClaimNames.Exp,
                ((DateTimeOffset)DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.AccessTokenExpires)))
                .ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64)
        ];

        return claim;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken
        (
            _jwtConfiguration.ValidIssuer,
            _jwtConfiguration.ValidAudience,
            claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.AccessTokenExpires)),
            signingCredentials: signingCredentials
        );
        return tokenOptions;
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = GetSymmetricSecurityKey(),
            ValidateLifetime = false,
            ValidIssuer = _jwtConfiguration.ValidIssuer,
            ValidAudience = _jwtConfiguration.ValidAudience
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }

    private static string CreateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    #endregion Private Methods
}