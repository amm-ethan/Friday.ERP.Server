using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Friday.ERP.Shared.DataTransferObjects;

namespace Friday.ERP.Core.Data.Entities.InventoryManagement;

[Table("im_category")]
public class Category
{
    [Key] public Guid Guid { get; set; }

    [Required]
    [Column("name", TypeName = "varchar(256)")]
    public string? Name { get; set; }

    [Required]
    [Column("color", TypeName = "varchar(256)")]
    public string? Color { get; set; }

    [Required] [Column("is_active")] public bool IsActive { get; set; }

    #region Foreign Keys

    public virtual ICollection<Product>? Products { get; set; }

    #endregion

    public static Category FromCategoryCreateDto(CategoryCreateDto categoryCreateDto)
    {
        return new Category
        {
            Guid = Guid.NewGuid(),
            Name = categoryCreateDto.Name,
            Color = categoryCreateDto.Color,
            IsActive = true
        };
    }
}