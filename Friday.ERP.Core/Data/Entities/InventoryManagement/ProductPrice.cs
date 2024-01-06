using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Friday.ERP.Core.Data.Entities.InvoiceManagement;
using Friday.ERP.Shared.DataTransferObjects;

namespace Friday.ERP.Core.Data.Entities.InventoryManagement;

[Table("im_product_price")]
public class ProductPrice
{
    [Key] public Guid Guid { get; set; }
    [Required] [Column("sale_price")] public long SalePrice { get; set; }
    [Required] [Column("is_whole_sale")] public bool IsWholeSale { get; set; }
    [Required] [Column("is_active")] public bool IsActive { get; set; }
    [Required] [Column("action_at")] public DateTime ActionAt { get; set; } = DateTime.Now;

    public static ProductPrice FromProductPriceCreateDto(
        ProductPriceCreateDto productPriceCreateDto)
    {
        return new ProductPrice
        {
            Guid = Guid.NewGuid(),
            SalePrice = productPriceCreateDto.SalePrice,
            IsWholeSale = productPriceCreateDto.IsWholeSale,
            IsActive = true,
            ActionAt = DateTime.Now
        };
    }

    #region Foreign Keys

    [Column("product_guid")] public Guid? ProductGuid { get; set; }

    [Required] [JsonIgnore] public Product? Product { get; set; }

    public virtual ICollection<InvoiceSaleProduct>? InvoiceProduct { get; set; }

    #endregion
}