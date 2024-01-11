using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Friday.ERP.Core.Data.Entities.InventoryManagement;
using Friday.ERP.Shared.DataTransferObjects;
using Newtonsoft.Json;

namespace Friday.ERP.Core.Data.Entities.InvoiceManagement;

[Table("lm_invoice_purchase_product")]
public class InvoicePurchaseProduct
{
    [Key] public Guid Guid { get; set; }

    [Required] [Column("quantity")] public int Quantity { get; set; }

    [Required] [Column("purchased_price")] public long PurchasedPrice { get; set; }

    [Required] [Column("total")] public long Total { get; set; }

    [Required] [Column("purchased_at")] public DateTime PurchasedAt { get; set; }

    public static InvoicePurchaseProduct FromInvoicePurchaseProductCreateDto(
        InvoicePurchaseProductCreateDto invoicePurchaseProductCreateDto)
    {
        return new InvoicePurchaseProduct
        {
            Guid = Guid.NewGuid(),
            ProductGuid = invoicePurchaseProductCreateDto.ProductGuid,
            Quantity = invoicePurchaseProductCreateDto.Quantity,
            PurchasedPrice = invoicePurchaseProductCreateDto.BuyPrice,
            Total = invoicePurchaseProductCreateDto.Total,
            PurchasedAt = DateTime.Now
        };
    }

    #region Foreign Keys

    [Column("product_guid")] public Guid? ProductGuid { get; set; }

    [Required] [JsonIgnore] public Product? Product { get; set; }

    [Column("invoice_purchase_guid")] public Guid? InvoicePurchaseGuid { get; set; }

    [Required] [JsonIgnore] public InvoicePurchase? InvoicePurchase { get; set; }

    #endregion
}