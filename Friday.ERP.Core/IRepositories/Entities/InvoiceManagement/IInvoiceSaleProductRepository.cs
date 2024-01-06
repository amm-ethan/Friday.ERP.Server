using Friday.ERP.Core.Data.Entities.InvoiceManagement;
using Friday.ERP.Shared.DataTransferObjects;

namespace Friday.ERP.Core.IRepositories.Entities.InvoiceManagement;

public interface IInvoiceSaleProductRepository
{
    void CreateInvoiceSaleProduct(InvoiceSaleProduct invoiceSaleProduct);

    Task<List<TopSellingProductsViewDto>> GetTopSellingProductsFromInvoiceSaleGuids(List<Guid> invoiceSaleGuids);
}