using Friday.ERP.Core.Data.Entities.InvoiceManagement;

namespace Friday.ERP.Core.IRepositories.Entities.InvoiceManagement;

public interface IInvoicePurchaseProductRepository
{
    void CreateInvoicePurchaseProduct(InvoicePurchaseProduct invoicePurchaseProduct);

    Task<IEnumerable<InvoicePurchaseProduct>>
        GetTotalPurchaseInfoOfProductByProductGuidAndBetweenDate(Guid productGuid,
            DateTime fromDate,
            DateTime toDate, bool trackChanges);

    Task<long> GetLastPurchasedPriceOfProduct(Guid guid);
}