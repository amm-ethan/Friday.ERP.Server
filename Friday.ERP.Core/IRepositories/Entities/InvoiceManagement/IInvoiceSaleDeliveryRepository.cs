using Friday.ERP.Core.Data.Entities.InvoiceManagement;

namespace Friday.ERP.Core.IRepositories.Entities.InvoiceManagement;

public interface IInvoiceSaleDeliveryRepository
{
    void CreateInvoiceSaleDelivery(InvoiceSaleDelivery invoiceSaleDelivery);

    Task<InvoiceSaleDelivery?> GetInvoiceSaleDeliveryByGuid(Guid guid, bool trackChanges);
}