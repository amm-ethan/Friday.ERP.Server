using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Friday.ERP.Core.Data.Entities.CustomerVendorManagement;
using Friday.ERP.Shared.DataTransferObjects;
using Newtonsoft.Json;
using DiscountType = Friday.ERP.Shared.Enums.DiscountTypeEnum;

namespace Friday.ERP.Core.Data.Entities.InvoiceManagement;

[Table("lm_invoice_purchase")]
public class InvoicePurchase
{
    [Key] public Guid Guid { get; set; }

    [Required]
    [Column("invoice_no", TypeName = "varchar(50)")]
    public string? InvoiceNo { get; set; }

    [Required] [Column("sub_total")] public long SubTotal { get; set; }

    [Column("discount")] public long Discount { get; set; }

    [Column("discount_type")] public DiscountType? DiscountType { get; set; }
    
    [Column("delivery_fees")] public long DeliveryFees { get; set; }
    
    [Column("other_fees")] public long OtherFees { get; set; }

    [Required] [Column("grand_total")] public long GrandTotal { get; set; }

    [Required] [Column("paid_total")] public long PaidTotal { get; set; }

    [Column("remark")] public string? Remark { get; set; }

    [Required]
    [Column("credit_debit_left")]
    public long CreditDebitLeft { get; set; }

    [Required] [Column("is_paid")] public bool IsPaid { get; set; }

    [Required] [Column("purchased_at")] public DateTime PurchasedAt { get; set; }

    public static InvoicePurchase FromInvoicePurchaseCreateDto(InvoicePurchaseCreateDto invoicePurchaseCreateDto)
    {
        return new InvoicePurchase
        {
            Guid = Guid.NewGuid(),
            SubTotal = invoicePurchaseCreateDto.SubTotal,
            Discount = invoicePurchaseCreateDto.Discount ?? 0,
            DiscountType = invoicePurchaseCreateDto.DiscountType,
            Total = invoicePurchaseCreateDto.Total,
            PaidTotal = invoicePurchaseCreateDto.PaidTotal,
            CreditDebitLeft = invoicePurchaseCreateDto.CreditDebitLeft,
            IsPaid = invoicePurchaseCreateDto.IsPaid,
            Remark = invoicePurchaseCreateDto.Remark,
            PurchasedAt = DateTime.Now
        };
    }

    #region Foreign Keys

    [Column("vendor_guid")] public Guid VendorGuid { get; set; }

    [JsonIgnore] public CustomerVendor? Vendor { get; set; }

    public virtual ICollection<InvoicePurchaseProduct>? PurchasedProducts { get; set; }

    #endregion
}