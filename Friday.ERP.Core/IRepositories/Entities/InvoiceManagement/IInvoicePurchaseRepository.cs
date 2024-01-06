using Friday.ERP.Core.Data.Entities.InvoiceManagement;
using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.RequestFeatures;

namespace Friday.ERP.Core.IRepositories.Entities.InvoiceManagement;

public interface IInvoicePurchaseRepository
{
    void CreateInvoicePurchase(InvoicePurchase invoicePurchase);

    Task<InvoicePurchase?> GetInvoicePurchaseByGuid(Guid guid, bool trackChanges);

    Task<PagedList<InvoicePurchase>> GetAllInvoicePurchases(InvoiceParameter invoiceParameter, bool trackChanges);

    Task<long> GetTotalPurchaseOutcomeBetweenDate(DateTime fromDate, DateTime toDate, bool trackChanges);

    Task<string?> GetCurrentInvoiceId(bool trackChanges);

    Task<long> GetPurchaseTotalOfSelectedMonthAndYear(int month, int year);
}