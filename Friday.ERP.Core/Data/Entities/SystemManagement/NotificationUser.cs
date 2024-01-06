using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Friday.ERP.Core.Data.Entities.AccountManagement;
using Newtonsoft.Json;

namespace Friday.ERP.Core.Data.Entities.SystemManagement;

[Table("sm_notification_user")]
public class NotificationUser
{
    [Key] public Guid Guid { get; set; }

    [Required] [Column("have_read")] public bool? HaveRead { get; set; } = false;

    # region Foreign Keys

    [Required]
    [Column("notification_guid")]
    public Guid NotificationGuid { get; set; }

    [Required] [JsonIgnore] public Notification? Notification { get; set; }

    [Required] [Column("user_guid")] public Guid? UserGuid { get; set; }

    [Required] [JsonIgnore] public User? User { get; set; }

    #endregion
}