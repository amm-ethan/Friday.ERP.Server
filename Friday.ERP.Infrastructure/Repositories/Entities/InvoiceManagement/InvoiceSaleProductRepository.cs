using Friday.ERP.Core.Data.Entities.InvoiceManagement;
using Friday.ERP.Core.IRepositories.Entities.InvoiceManagement;
using Friday.ERP.Shared.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace Friday.ERP.Infrastructure.Repositories.Entities.InvoiceManagement;

internal sealed class InvoiceSaleProductRepository(RepositoryContext repositoryContext)
    : RepositoryBase<InvoiceSaleProduct>(repositoryContext), IInvoiceSaleProductRepository
{
    public void CreateInvoiceSaleProduct(InvoiceSaleProduct invoiceSaleProduct)
    {
        Create(invoiceSaleProduct);
    }

    public async Task<List<TopSellingProductsViewDto>> GetTopSellingProductsFromInvoiceSaleGuids(
        List<Guid> invoiceSaleGuids)
    {
        return await FindByCondition(x => invoiceSaleGuids.Contains(x.InvoiceSaleGuid)
                                          && x.Product != null, false)
            .Include(c => c.Product!)
            .ThenInclude(c => c.Category)
            .GroupBy(x => x.ProductGuid)
            .Where(group => group.Any())
            .Select(group => new TopSellingProductsViewDto
            (
                group.Key,
                group.First().Product!.Name!,
                group.First().Product!.Category!.Name!,
                group.First().Product!.Category!.Color!,
                group.Count(),
                group.Sum(x => x.Quantity)
            ))
            .ToListAsync();
    }
}