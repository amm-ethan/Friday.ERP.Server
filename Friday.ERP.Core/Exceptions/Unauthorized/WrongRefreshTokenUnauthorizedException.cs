namespace Friday.ERP.Core.Exceptions.Unauthorized;

public sealed class WrongRefreshTokenUnauthorizedException() : UnauthorizedException("Invalid Refresh Token.");