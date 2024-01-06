using Friday.ERP.Core.IRepositories.Entities.AccountManagement;
using Friday.ERP.Core.IRepositories.Entities.CustomerVendorManagement;
using Friday.ERP.Core.IRepositories.Entities.InventoryManagement;
using Friday.ERP.Core.IRepositories.Entities.InvoiceManagement;
using Friday.ERP.Core.IRepositories.Entities.SystemManagement;

namespace Friday.ERP.Core.IRepositories;

public interface IRepositoryManager
{
    IUserLoginRepository UserLogin { get; }
    IUserRepository User { get; }
    IUserRoleRepository UserRole { get; }

    ICustomerVendorRepository CustomerVendor { get; }

    ICategoryRepository Category { get; }
    IProductRepository Product { get; }
    IProductPriceRepository ProductPrice { get; }

    IInvoicePurchaseRepository InvoicePurchase { get; }
    IInvoicePurchaseProductRepository InvoicePurchaseProduct { get; }
    IInvoiceSaleRepository InvoiceSale { get; }
    IInvoiceSaleProductRepository InvoiceSaleProduct { get; }
    IInvoiceSaleDeliveryRepository InvoiceSaleDelivery { get; }

    INotificationRepository Notification { get; }
    INotificationUserRepository NotificationUser { get; }
    ISettingRepository Setting { get; }

    Task BeginTransaction();
    Task SaveAsync();
}