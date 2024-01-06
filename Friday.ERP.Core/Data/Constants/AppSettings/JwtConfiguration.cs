namespace Friday.ERP.Core.Data.Constants.AppSettings;

public record JwtConfiguration
{
    public string? ValidIssuer { get; set; }
    public string? ValidAudience { get; set; }
    public string? AccessTokenExpires { get; set; }
    public string? RefreshTokenExpires { get; set; }
    public string? JwtSecret { get; set; }
}