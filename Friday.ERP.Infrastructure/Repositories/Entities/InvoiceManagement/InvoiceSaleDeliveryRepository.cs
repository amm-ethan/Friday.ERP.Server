using Friday.ERP.Core.Data.Entities.InvoiceManagement;
using Friday.ERP.Core.IRepositories.Entities.InvoiceManagement;
using Microsoft.EntityFrameworkCore;

namespace Friday.ERP.Infrastructure.Repositories.Entities.InvoiceManagement;

internal sealed class InvoiceSaleDeliveryRepository(RepositoryContext repositoryContext)
    : RepositoryBase<InvoiceSaleDelivery>(repositoryContext), IInvoiceSaleDeliveryRepository
{
    public void CreateInvoiceSaleDelivery(InvoiceSaleDelivery invoiceSaleDelivery)
    {
        Create(invoiceSaleDelivery);
    }

    public async Task<InvoiceSaleDelivery?> GetInvoiceSaleDeliveryByGuid(Guid guid, bool trackChanges)
    {
        return await FindByCondition(c => c.Guid.Equals(guid), trackChanges).SingleOrDefaultAsync();
    }
}