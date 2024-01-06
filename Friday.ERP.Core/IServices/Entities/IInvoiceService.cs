using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.RequestFeatures;

namespace Friday.ERP.Core.IServices.Entities;

public interface IInvoiceService
{
    Task<InvoicePurchaseViewDto> CreateInvoicePurchase(InvoicePurchaseCreateDto invoicePurchaseCreateDto,
        string wwwroot);

    Task<InvoicePurchaseViewDto> UpdateInvoicePurchase(Guid guid, InvoicePurchaseUpdateDto invoicePurchaseUpdateDto,
        string wwwroot, string currentUserGuid);

    Task<(List<InvoicePurchaseTableViewDto>, MetaData metaData)> GetAllInvoicePurchases(
        InvoiceParameter invoiceParameter);

    Task<InvoicePurchaseViewDto> GetInvoicePurchaseByGuid(Guid guid, string wwwroot);

    Task<InvoiceSaleViewDto> CreateInvoiceSale(InvoiceSaleCreateDto invoiceSaleCreateDto, string wwwroot);

    Task<InvoiceSaleViewDto> UpdateInvoiceSale(Guid guid, InvoiceSaleUpdateDto invoiceSaleUpdateDto,
        string wwwroot, string currentUserGuid);

    Task<(List<InvoiceSaleTableViewDto>, MetaData metaData)> GetAllInvoiceSales(
        InvoiceParameter invoiceParameter);

    Task<InvoiceSaleViewDto> GetInvoiceSaleByGuid(Guid guid, string wwwroot);

    Task<InvoiceSaleDeliveryViewDto> UpdateInvoiceSaleDeliveryByGuid(Guid guid,
        InvoiceSaleDeliveryUpdateDto invoiceSaleDeliveryUpdateDto, string currentUserGuid);

    Task<List<InvoiceSaleForFrequentCustomerViewDto>> GetFrequentCustomersFromInvoiceSale(DateTime fromLastDays);

    Task<List<TopSellingProductsViewDto>> GetTopSellingProductsFromInvoiceSale(DateTime fromLastDays);

    Task<List<MonthlySaleFigureViewDto>> GetMonthlySummary(int lastTotalMonths = 12);

    Task<ProductPurchasePriceViewDto> GetLastPurchasedPriceOfProduct(Guid guid);

    Task<InvoiceIdViewDto> GetNextPurchaseInvoiceNumber(bool isSale);
}