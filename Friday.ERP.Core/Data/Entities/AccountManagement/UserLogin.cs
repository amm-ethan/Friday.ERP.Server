using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Friday.ERP.Core.Data.Entities.AccountManagement;

[Table("am_user_login")]
public class UserLogin
{
    [Key] public Guid Guid { get; set; }

    [Required]
    [Column("access_token", TypeName = "varchar(512)")]
    public string? AccessToken { get; set; }

    [Required]
    [Column("refresh_token", TypeName = "varchar(512)")]
    public string? RefreshToken { get; set; }

    [Required]
    [Column("refresh_token_expiry_at")]
    public DateTime RefreshTokenExpiryAt { get; set; }

    # region Foreign Keys

    [Column("user_guid")] public Guid? UserGuid { get; set; }

    [Required] [JsonIgnore] public User? User { get; set; }

    #endregion
}