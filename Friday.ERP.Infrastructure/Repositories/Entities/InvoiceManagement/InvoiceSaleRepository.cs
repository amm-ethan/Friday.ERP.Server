using Friday.ERP.Core.Data.Entities.InvoiceManagement;
using Friday.ERP.Core.IRepositories.Entities.InvoiceManagement;
using Friday.ERP.Infrastructure.Utilities.Entities;
using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Friday.ERP.Infrastructure.Repositories.Entities.InvoiceManagement;

internal sealed class InvoiceSaleRepository(RepositoryContext repositoryContext)
    : RepositoryBase<InvoiceSale>(repositoryContext), IInvoiceSaleRepository
{
    public void CreateInvoiceSale(InvoiceSale invoiceSale)
    {
        Create(invoiceSale);
    }

    public async Task<InvoiceSale?> GetInvoiceSaleByGuid(Guid guid, bool trackChanges)
    {
        return await FindByCondition(c => c.Guid.Equals(guid), trackChanges)
            .Include(c => c.Customer)
            .Include(c => c.InvoiceSaleDelivery)
            .Include(c => c.SoldProducts!)
            .ThenInclude(c => c.ProductPrice)
            .Include(c => c.SoldProducts!)
            .ThenInclude(x => x.Product)
            .SingleOrDefaultAsync();
    }

    public async Task<PagedList<InvoiceSale>> GetAllInvoiceSales(InvoiceParameter invoiceParameter, bool trackChanges)
    {
        var invoiceSales = await FindAll(trackChanges)
            .Include(c => c.Customer)
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

        return PagedList<InvoiceSale>.ToPagedList(invoiceSales, count, invoiceParameter.PageNumber,
            invoiceParameter.PageSize);
    }

    public async Task<string?> GetCurrentInvoiceId(bool trackChanges)
    {
        return await FindAll(trackChanges)
            .OrderByDescending(c => c.InvoiceNo)
            .Select(c => c.InvoiceNo!)
            .FirstOrDefaultAsync();
    }

    public async Task<List<InvoiceSaleForFrequentCustomerViewDto>> GetFrequentCustomersFromInvoiceSale(
        DateTime fromLastDays)
    {
        return await FindByCondition(x => x.PurchasedAt >= fromLastDays && x.Customer != null, false)
            .Include(c => c.Customer)
            .GroupBy(x => x.CustomerGuid)
            .Where(group => group.Any())
            .Select(group => new InvoiceSaleForFrequentCustomerViewDto
            (
                group.Key ?? Guid.Empty,
                group.First().Customer!.Name,
                group.Count(),
                group.Sum(x => x.Total)
            ))
            .ToListAsync();
    }

    public async Task<List<Guid>> GetInvoiceSaleGuidsFromLastDays(DateTime fromLastDays)
    {
        return await FindByCondition(x => x.PurchasedAt >= fromLastDays, false)
            .OrderByDescending(c => c.InvoiceNo)
            .Select(c => c.Guid)
            .ToListAsync();
    }

    public async Task<long> GetSaleTotalOfSelectedMonthAndYear(int month, int year)
    {
        return await FindByCondition(x => x.PurchasedAt.Month == month && x.PurchasedAt.Year == year, false)
            .SumAsync(inv => inv.Total);
    }
}