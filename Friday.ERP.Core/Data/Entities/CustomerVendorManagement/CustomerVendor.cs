using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Friday.ERP.Core.Data.Entities.InvoiceManagement;
using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.Enums;

namespace Friday.ERP.Core.Data.Entities.CustomerVendorManagement;

[Table("cm_customer_vendor")]
public class CustomerVendor
{
    [Key] public Guid Guid { get; set; }

    [Required]
    [Column("code", TypeName = "varchar(256)")]
    public string? Code { get; set; }

    [Required]
    [Column("name", TypeName = "varchar(256)")]
    public string? Name { get; set; }

    [Required]
    [Phone]
    [Column("phone", TypeName = "varchar(15)")]
    public string? Phone { get; set; }

    [EmailAddress]
    [Column("email", TypeName = "varchar(50)")]
    public string? Email { get; set; }

    [Column("address", TypeName = "varchar(50)")]
    public string? Address { get; set; }

    [Required]
    [Column("customer_vendor_type")]
    public CustomerVendorTypeEnum? CustomerVendorType { get; set; }

    [Required]
    [Column("total_credit_debit_left")]
    // Negative if debit, positive if credit
    public long TotalCreditDebitLeft { get; set; }

    public static CustomerVendor FromCustomerVendorCreateDto(CustomerVendorCreateDto customerVendorCreateDto)
    {
        return new CustomerVendor
        {
            Guid = Guid.NewGuid(),
            Name = customerVendorCreateDto.Name,
            Phone = customerVendorCreateDto.Phone,
            Email = customerVendorCreateDto.Email,
            Address = customerVendorCreateDto.Address,
            TotalCreditDebitLeft = customerVendorCreateDto.TotalCreditDebitLeft
        };
    }

    # region Foreign Keys

    public virtual ICollection<InvoicePurchase>? InvoicePurchases { get; set; }

    public virtual ICollection<InvoiceSale>? InvoiceSales { get; set; }

    # endregion
}