namespace Friday.ERP.Shared.DataTransferObjects;

public record LoginDto(
    string Username,
    byte[] Password
);

public record LoginResultDto(
    string AccessToken,
    string RefreshToken,
    DateTime RefreshTokenExpiredAt
);

public record RefreshTokenDto(
    string AccessToken,
    string RefreshToken
);