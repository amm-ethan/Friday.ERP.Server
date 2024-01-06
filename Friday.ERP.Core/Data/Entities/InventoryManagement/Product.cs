using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Friday.ERP.Core.Data.Entities.InvoiceManagement;
using Friday.ERP.Shared.DataTransferObjects;

namespace Friday.ERP.Core.Data.Entities.InventoryManagement;

[Table("im_product")]
public class Product
{
    [Key] public Guid Guid { get; set; }

    [Required]
    [Column("code", TypeName = "varchar(256)")]
    public string? Code { get; set; }

    [Column("image")] public string? Image { get; set; }

    [Required]
    [Column("name", TypeName = "varchar(256)")]
    public string? Name { get; set; }

    [Required] [Column("stock")] public int Stock { get; set; }

    [Required] [Column("is_active")] public bool IsActive { get; set; }

    public static Product FromProductCreateDto(ProductCreateDto productCreateDto)
    {
        return new Product
        {
            Guid = Guid.NewGuid(),
            Code = productCreateDto.Code,
            Name = productCreateDto.Name,
            Stock = productCreateDto.TotalStockLeft,
            IsActive = true
        };
    }

    #region Foreign Keys

    [Column("category_guid")] public Guid? CategoryGuid { get; set; }

    [Required] [JsonIgnore] public Category? Category { get; set; }

    public virtual ICollection<ProductPrice>? ProductPrices { get; set; }

    public virtual ICollection<InvoicePurchaseProduct>? InvoicePurchaseProducts { get; set; }
    public virtual ICollection<InvoiceSaleProduct>? InvoiceSaleProducts { get; set; }

    #endregion
}