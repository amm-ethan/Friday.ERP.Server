using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Friday.ERP.Core.Data.Entities.CustomerVendorManagement;
using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.Enums;
using Newtonsoft.Json;

namespace Friday.ERP.Core.Data.Entities.InvoiceManagement;

[Table("lm_invoice_sale")]
public class InvoiceSale
{
    [Key] public Guid Guid { get; set; }

    [Required]
    [Column("invoice_no", TypeName = "varchar(50)")]
    public string? InvoiceNo { get; set; }

    [Required] [Column("purchased_at")] public DateTime PurchasedAt { get; set; }

    [Required] [Column("sub_total")] public long SubTotal { get; set; }

    [Column("discount")] public long Discount { get; set; }

    [Required] [Column("total")] public long Total { get; set; }

    [Required] [Column("paid_total")] public long PaidTotal { get; set; }

    [Column("remark")] public string? Remark { get; set; }

    [Required]
    [Column("credit_debit_left")]
    public long CreditDebitLeft { get; set; }

    [Column("discount_type")] public DiscountTypeEnum? DiscountType { get; set; }

    public static InvoiceSale FromInvoiceSaleCreateDto(InvoiceSaleCreateDto invoiceSaleCreateDto)
    {
        return new InvoiceSale
        {
            Guid = Guid.NewGuid(),
            SubTotal = invoiceSaleCreateDto.SubTotal,
            Discount = invoiceSaleCreateDto.Discount ?? 0,
            DiscountType = invoiceSaleCreateDto.DiscountType,
            Total = invoiceSaleCreateDto.Total,
            PaidTotal = invoiceSaleCreateDto.PaidTotal,
            CreditDebitLeft = invoiceSaleCreateDto.CreditDebitLeft,
            Remark = invoiceSaleCreateDto.Remark,
            PurchasedAt = DateTime.Now
        };
    }

    #region Foreign Keys

    [Column("customer_guid")] public Guid? CustomerGuid { get; set; }

    [JsonIgnore] public CustomerVendor? Customer { get; set; }

    public virtual ICollection<InvoiceSaleProduct>? SoldProducts { get; set; }

    [JsonIgnore] public InvoiceSaleDelivery? InvoiceSaleDelivery { get; set; }

    #endregion
}