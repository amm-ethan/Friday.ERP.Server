using Friday.ERP.Core.Data.Entities.InvoiceManagement;
using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.RequestFeatures;

namespace Friday.ERP.Core.IRepositories.Entities.InvoiceManagement;

public interface IInvoiceSaleRepository
{
    void CreateInvoiceSale(InvoiceSale invoiceSale);

    Task<InvoiceSale?> GetInvoiceSaleByGuid(Guid guid, bool trackChanges);

    Task<PagedList<InvoiceSale>> GetAllInvoiceSales(InvoiceParameter invoiceParameter, bool trackChanges);

    Task<string?> GetCurrentInvoiceId(bool trackChanges);
    Task<List<InvoiceSaleForFrequentCustomerViewDto>> GetFrequentCustomersFromInvoiceSale(DateTime fromLastDays);

    Task<List<Guid>> GetInvoiceSaleGuidsFromLastDays(DateTime fromLastDays);

    Task<long> GetSaleTotalOfSelectedMonthAndYear(int month, int year);
}