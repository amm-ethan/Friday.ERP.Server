using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Friday.ERP.Shared.DataTransferObjects;

namespace Friday.ERP.Core.Data.Entities.SystemManagement;

[Table("sm_notification")]
public class Notification
{
    [Key] public Guid Guid { get; set; }

    [Required]
    [Column("heading", TypeName = "varchar(255)")]
    public string? Heading { get; set; }

    [Required]
    [Column("body", TypeName = "varchar(255)")]
    public string? Body { get; set; }

    [Column("is_system_wide")] public bool? IsSystemWide { get; set; } = false;

    [Column("sent_at")] public DateTime SentAt { get; set; } = DateTime.Now;

    # region Foreign Keys

    public virtual ICollection<NotificationUser>? NotificationUsers { get; set; }

    #endregion

    public static Notification FromNotificationCreateDto(NotificationCreateDto notificationCreateDto,
        bool isSystemWide)
    {
        return new Notification
        {
            Guid = Guid.NewGuid(),
            Heading = notificationCreateDto.Heading,
            Body = notificationCreateDto.Body,
            IsSystemWide = isSystemWide,
            SentAt = DateTime.Now
        };
    }
}