using Friday.ERP.Core.Data.Entities.InvoiceManagement;
using Friday.ERP.Core.IRepositories.Entities.InvoiceManagement;
using Friday.ERP.Infrastructure.Utilities.Entities;
using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Friday.ERP.Infrastructure.Repositories.Entities.InvoiceManagement;

internal sealed class InvoicePurchaseRepository(RepositoryContext repositoryContext)
    : RepositoryBase<InvoicePurchase>(repositoryContext), IInvoicePurchaseRepository
{
    public void CreateInvoicePurchase(InvoicePurchase invoicePurchase)
    {
        Create(invoicePurchase);
    }

    public async Task<InvoicePurchase?> GetInvoicePurchaseByGuid(Guid guid, bool trackChanges)
    {
        return await FindByCondition(c => c.Guid.Equals(guid), trackChanges)
            .Include(c => c.Vendor)
            .Include(c => c.PurchasedProducts!)
            .ThenInclude(x => x.Product)
            .SingleOrDefaultAsync();
    }

    public async Task<PagedList<InvoicePurchase>> GetAllInvoicePurchases(InvoiceParameter invoiceParameter,
        bool trackChanges)
    {
        var invoicePurchases = await FindAll(trackChanges)
            .Include(c => c.Vendor)
            .Filter(invoiceParameter.CustomerVendorGuid, invoiceParameter.FromPurchasedDate,
                invoiceParameter.ToPurchasedDate)
            .Search(invoiceParameter.SearchTerm)
            .Sort(invoiceParameter.OrderBy!)
            .Skip((invoiceParameter.PageNumber - 1) * invoiceParameter.PageSize)
            .Take(invoiceParameter.PageSize)
            .ToListAsync();

        var count = await FindAll(trackChanges)
            .Filter(invoiceParameter.CustomerVendorGuid, invoiceParameter.FromPurchasedDate,
                invoiceParameter.ToPurchasedDate)
            .Search(invoiceParameter.SearchTerm)
            .Sort(invoiceParameter.OrderBy!).CountAsync();

        return PagedList<InvoicePurchase>.ToPagedList(invoicePurchases, count, invoiceParameter.PageNumber,
            invoiceParameter.PageSize);
    }

    public async Task<long> GetTotalPurchaseOutcomeBetweenDate(DateTime fromDate, DateTime toDate, bool trackChanges)
    {
        return await FindByCondition(
                c => c.PurchasedAt >= fromDate && c.PurchasedAt <= toDate, trackChanges)
            .SumAsync(c => c.Total);
    }

    public async Task<string?> GetCurrentInvoiceId(bool trackChanges)
    {
        return await FindAll(trackChanges)
            .OrderByDescending(c => c.InvoiceNo)
            .Select(c => c.InvoiceNo!)
            .FirstOrDefaultAsync();
    }

    public async Task<long> GetPurchaseTotalOfSelectedMonthAndYear(int month, int year)
    {
        return await FindByCondition(x => x.PurchasedAt.Month == month && x.PurchasedAt.Year == year, false)
            .SumAsync(inv => inv.Total);
    }
}