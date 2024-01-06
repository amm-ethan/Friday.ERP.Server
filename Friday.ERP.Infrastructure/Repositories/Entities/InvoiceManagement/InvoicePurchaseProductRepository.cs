using Friday.ERP.Core.Data.Entities.InvoiceManagement;
using Friday.ERP.Core.IRepositories.Entities.InvoiceManagement;
using Microsoft.EntityFrameworkCore;

namespace Friday.ERP.Infrastructure.Repositories.Entities.InvoiceManagement;

internal sealed class InvoicePurchaseProductRepository(RepositoryContext repositoryContext)
    : RepositoryBase<InvoicePurchaseProduct>(repositoryContext), IInvoicePurchaseProductRepository
{
    public void CreateInvoicePurchaseProduct(InvoicePurchaseProduct invoicePurchaseProduct)
    {
        Create(invoicePurchaseProduct);
    }

    public async Task<IEnumerable<InvoicePurchaseProduct>> GetTotalPurchaseInfoOfProductByProductGuidAndBetweenDate(
        Guid productGuid, DateTime fromDate, DateTime toDate, bool trackChanges)
    {
        return await FindByCondition(
                c => c.PurchasedAt >= fromDate
                     && c.PurchasedAt <= toDate
                     && c.ProductGuid == productGuid, trackChanges)
            .ToListAsync();
    }

    public async Task<long> GetLastPurchasedPriceOfProduct(Guid guid)
    {
        return await FindAll(false)
            .OrderByDescending(c => c.PurchasedAt)
            .Select(c => c.PurchasedPrice).FirstOrDefaultAsync();
    }
}