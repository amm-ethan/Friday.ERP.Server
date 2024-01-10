using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Friday.ERP.Shared.DataTransferObjects;
using Newtonsoft.Json;

namespace Friday.ERP.Core.Data.Entities.InvoiceManagement;

[Table("lm_invoice_sale_delivery")]
public class InvoiceSaleDelivery
{
    [Key] public Guid Guid { get; set; }

    [Column("delivery_service_name", TypeName = "varchar(50)")]
    public string? DeliveryServiceName { get; set; }

    [Column("delivery_contact_person", TypeName = "varchar(50)")]
    public string? DeliveryContactPerson { get; set; }

    [Column("delivery_contact_phone", TypeName = "varchar(50)")]
    public string? DeliveryContactPhone { get; set; }

    [Column("remark", TypeName = "varchar(50)")]
    public string? Remark { get; set; }

    public static InvoiceSaleDelivery FromInvoiceSaleDeliveryCreateDto(
        InvoiceSaleDeliveryCreateDto invoiceSaleProductCreateDto)
    {
        return new InvoiceSaleDelivery
        {
            Guid = Guid.NewGuid(),
            DeliveryServiceName = invoiceSaleProductCreateDto.DeliveryServiceName,
            DeliveryContactPerson = invoiceSaleProductCreateDto.DeliveryContactPerson,
            DeliveryContactPhone = invoiceSaleProductCreateDto.DeliveryContactPhone,
            Remark = invoiceSaleProductCreateDto.Remark,
        };
    }

    #region Foreign Keys

    [Column("invoice_sale_guid")] public Guid? InvoiceSaleGuid { get; set; }

    [Required] [JsonIgnore] public InvoiceSale? InvoiceSale { get; set; }

    #endregion
}