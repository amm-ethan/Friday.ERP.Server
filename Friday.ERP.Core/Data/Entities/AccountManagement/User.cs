using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Friday.ERP.Core.Data.Entities.SystemManagement;
using Friday.ERP.Shared.DataTransferObjects;

namespace Friday.ERP.Core.Data.Entities.AccountManagement;

[Table("am_user")]
public class User
{
    [Key] public Guid Guid { get; set; }

    [Required]
    [Column("name", TypeName = "varchar(50)")]
    public string? Name { get; set; }

    [Phone]
    [Column("phone", TypeName = "varchar(50)")]
    public string? Phone { get; set; }

    [Column("email", TypeName = "varchar(50)")]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [Column("username", TypeName = "varchar(10)")]
    public string? Username { get; set; }

    [Required]
    [Column("password", TypeName = "varchar(100)")]
    public string? Password { get; set; }

    public static User FromUserCreateDto(UserCreateDto userCreateDto)
    {
        return new User
        {
            Guid = Guid.NewGuid(),
            Name = userCreateDto.Name,
            Phone = userCreateDto.Phone,
            Email = userCreateDto.Email,
            Username = userCreateDto.Username
        };
    }

    # region Foreign Keys

    [Column("user_role_guid")] public Guid? UserRoleGuid { get; set; }

    [Required] [JsonIgnore] public UserRole? UserRole { get; set; }
    public virtual UserLogin? UserLogin { get; set; }
    public virtual ICollection<NotificationUser>? NotificationUsers { get; set; }

    #endregion
}