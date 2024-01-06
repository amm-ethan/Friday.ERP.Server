using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Friday.ERP.Core.Data.Entities.AccountManagement;

[Table("am_user_role")]
public class UserRole
{
    [Key] public Guid Guid { get; set; }

    [Required]
    [Column("name", TypeName = "varchar(50)")]
    public string? Name { get; set; }

    # region Foreign Keys

    public virtual ICollection<User>? Users { get; set; }

    #endregion
}