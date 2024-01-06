using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Friday.ERP.Core.Data.Entities.InventoryManagement;
using Friday.ERP.Shared.DataTransferObjects;
using Newtonsoft.Json;

namespace Friday.ERP.Core.Data.Entities.InvoiceManagement;

[Table("lm_invoice_sale_product")]
public class InvoiceSaleProduct
{
    [Key] public Guid Guid { get; set; }

    [Required] [Column("quantity")] public int Quantity { get; set; }

    [Required] [Column("total_price")] public long TotalPrice { get; set; }

    public static InvoiceSaleProduct FromInvoiceSaleProductCreateDto(
        InvoiceSaleProductCreateDto invoiceSaleProductCreateDto)
    {
        return new InvoiceSaleProduct
        {
            Guid = Guid.NewGuid(),
            ProductGuid = invoiceSaleProductCreateDto.ProductGuid,
            Quantity = invoiceSaleProductCreateDto.Quantity,
            TotalPrice = invoiceSaleProductCreateDto.TotalPrice
        };
    }

    #region Foreign Keys

    [Column("product_guid")] public Guid ProductGuid { get; set; }

    [Required] [JsonIgnore] public Product? Product { get; set; }

    [Column("invoice_sale_guid")] public Guid InvoiceSaleGuid { get; set; }

    [Required] [JsonIgnore] public InvoiceSale? InvoiceSale { get; set; }

    [Column("product_price_guid")] public Guid ProductPriceGuid { get; set; }

    [Required] [JsonIgnore] public ProductPrice? ProductPrice { get; set; }

    #endregion
}