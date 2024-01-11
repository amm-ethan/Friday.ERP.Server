using Friday.ERP.Shared.Enums;
using Friday.ERP.Shared.RequestFeatures;

namespace Friday.ERP.Shared.DataTransferObjects;

#region Invoice_Purchase

public record InvoicePurchaseProductCreateDto(
    Guid ProductGuid,
    long BuyPrice,
    int Quantity,
    long Total);

public record InvoicePurchaseProductViewDto(
    Guid Guid,
    Guid ProductGuid,
    string? Image,
    string ProductName,
    int Quantity,
    long BuyPrice,
    long Total);

public record InvoicePurchaseCreateDto(
    long SubTotal,
    long Discount,
    DiscountTypeEnum? DiscountType,
    long DeliveryFees,
    long Total,
    long GrandTotal,
    long PaidTotal,
    long CreditDebitLeft,
    bool IsPaid,
    string? Remark,
    Guid VendorGuid,
    List<InvoicePurchaseProductCreateDto> PurchasedProducts);

public record InvoicePurchaseViewDto(
    Guid Guid,
    string InvoiceNo,
    long SubTotal,
    long Discount,
    DiscountTypeEnum? DiscountType,
    long DeliveryFees,
    long Total,
    long GrandTotal,
    long PaidTotal,
    long CreditDebitLeft,
    bool IsPaid,
    string? Remark,
    DateTime PurchasedAt,
    Guid VendorGuid,
    CustomerVendorViewDto Vendor,
    List<InvoicePurchaseProductViewDto> PurchasedProducts);

public record InvoicePurchaseUpdateDto(
    long? Discount,
    DiscountTypeEnum? DiscountType,
    long? DeliveryFees,
    long? Total,
    long? GrandTotal,
    long? PaidTotal,
    long? CreditDebitLeft,
    bool? IsPaid);

public record InvoicePurchaseTableViewDto(
    Guid Guid,
    string InvoiceNo,
    long GrandTotal,
    Guid VendorGuid,
    string VendorName,
    DateTime PurchasedAt
);

public record InvoiceIdViewDto(
    string InvoiceNo
);

public record InvoicePurchasePreCreateDto(
    long Discount,
    DiscountTypeEnum? DiscountType,
    long DeliveryFees,
    long Total,
    long GrandTotal,
    long PaidTotal,
    long CreditDebitLeft,
    bool IsPaid,
    string? Remark
);

#endregion

#region Invoice_Sale

public record InvoiceSaleProductCreateDto(
    Guid ProductGuid,
    int Quantity,
    long Total,
    Guid ProducePriceGuid);

public record InvoiceSaleProductViewDto(
    Guid Guid,
    Guid ProductGuid,
    string? Image,
    string ProductName,
    int Quantity,
    long Total,
    Guid ProducePriceGuid,
    long ProducePriceSalePrice,
    bool? ProducePriceIsWholeSale
);

public record InvoiceSaleCreateDto(
    long SubTotal,
    long Discount,
    DiscountTypeEnum? DiscountType,
    long DeliveryFees,
    long Total,
    long GrandTotal,
    long PaidTotal,
    long CreditDebitLeft,
    string? Remark,
    Guid? CustomerGuid,
    List<InvoiceSaleProductCreateDto> SoldProducts,
    InvoiceSaleDeliveryCreateDto? DeliveryInfo
);

public record InvoiceSaleViewDto(
    Guid Guid,
    string InvoiceNo,
    long SubTotal,
    long Discount,
    DiscountTypeEnum? DiscountType,
    long DeliveryFees,
    long Total,
    long GrandTotal,
    long PaidTotal,
    long CreditDebitLeft,
    string? Remark,
    DateTime PurchasedAt,
    Guid? CustomerGuid,
    CustomerVendorViewDto? Customer,
    List<InvoiceSaleProductViewDto> SoldProducts,
    InvoiceSaleDeliveryViewDto? DeliveryInfo);

public record InvoiceSaleUpdateDto(
    long? Discount,
    DiscountTypeEnum? DiscountType,
    long? DeliveryFees,
    long? Total,
    long? GrandTotal,
    long? PaidTotal,
    long? CreditDebitLeft
);

public record InvoiceSaleDeliveryViewDto(
    Guid Guid,
    string DeliveryServiceName,
    string? DeliveryContactPerson,
    string? DeliveryContactPhone,
    string? Remark
);

public record InvoiceSaleDeliveryUpdateDto(
    string? DeliveryServiceName,
    string? DeliveryContactPerson,
    string? DeliveryContactPhone,
    string? Remark
);

public record InvoiceSaleDeliveryCreateDto(
    string DeliveryServiceName,
    string? DeliveryContactPerson,
    string? DeliveryContactPhone,
    string? Remark
);

public record InvoiceSaleTableViewDto(
    Guid Guid,
    string InvoiceNo,
    long GrandTotal,
    Guid? CustomerGuid,
    string? CustomerName,
    DateTime PurchasedAt
);

public record InvoiceSalePreCreateDto(
    long Discount,
    DiscountTypeEnum? DiscountType,
    long DeliveryFees,
    long Total,
    long GrandTotal,
    long PaidTotal,
    long CreditDebitLeft,
    string? Remark
);

#endregion

public class InvoiceParameter : RequestParameters
{
    public InvoiceParameter()
    {
        OrderBy = "InvoiceNo";
    }

    public string? SearchTerm { get; set; }

    public Guid? CustomerVendorGuid { get; set; }

    public DateTime? FromPurchasedDate { get; set; }

    public DateTime? ToPurchasedDate { get; set; }
}