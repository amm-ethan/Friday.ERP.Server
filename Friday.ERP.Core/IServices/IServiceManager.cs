using Friday.ERP.Core.IServices.Entities;

namespace Friday.ERP.Core.IServices;

public interface IServiceManager
{
    IAccountService AccountService { get; }

    IAuthenticationService AuthenticationService { get; }

    ICustomerVendorService CustomerVendorService { get; }

    IInventoryService InventoryService { get; }

    IInvoiceService InvoiceService { get; }

    ISystemService SystemService { get; }
}