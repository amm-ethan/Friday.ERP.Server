using Friday.ERP.Core.IRepositories;
using Friday.ERP.Core.IRepositories.Entities.AccountManagement;
using Friday.ERP.Core.IRepositories.Entities.CustomerVendorManagement;
using Friday.ERP.Core.IRepositories.Entities.InventoryManagement;
using Friday.ERP.Core.IRepositories.Entities.InvoiceManagement;
using Friday.ERP.Core.IRepositories.Entities.SystemManagement;
using Friday.ERP.Infrastructure.Repositories.Entities.AccountManagement;
using Friday.ERP.Infrastructure.Repositories.Entities.CustomerVendorManagement;
using Friday.ERP.Infrastructure.Repositories.Entities.InventoryManagement;
using Friday.ERP.Infrastructure.Repositories.Entities.InvoiceManagement;
using Friday.ERP.Infrastructure.Repositories.Entities.SystemManagement;

namespace Friday.ERP.Infrastructure.Repositories;

public sealed class RepositoryManager(RepositoryContext repositoryContext) : IRepositoryManager
{
    private readonly Lazy<ICategoryRepository> _categoryRepository =
        new(() => new CategoryRepository(repositoryContext));

    private readonly Lazy<ICustomerVendorRepository> _customerVendorRepository =
        new(() => new CustomerVendorRepository(repositoryContext));

    private readonly Lazy<IInvoicePurchaseProductRepository> _invoicePurchaseProductRepository =
        new(() => new InvoicePurchaseProductRepository(repositoryContext));

    private readonly Lazy<IInvoicePurchaseRepository> _invoicePurchaseRepository =
        new(() => new InvoicePurchaseRepository(repositoryContext));

    private readonly Lazy<IInvoiceSaleDeliveryRepository> _invoiceSaleDeliveryRepository =
        new(() => new InvoiceSaleDeliveryRepository(repositoryContext));

    private readonly Lazy<IInvoiceSaleProductRepository> _invoiceSaleProductRepository =
        new(() => new InvoiceSaleProductRepository(repositoryContext));

    private readonly Lazy<IInvoiceSaleRepository> _invoiceSaleRepository =
        new(() => new InvoiceSaleRepository(repositoryContext));

    private readonly Lazy<INotificationRepository> _notificationRepository =
        new(() => new NotificationRepository(repositoryContext));

    private readonly Lazy<INotificationUserRepository> _notificationUserRepository =
        new(() => new NotificationUserRepository(repositoryContext));

    private readonly Lazy<IProductPriceRepository> _productPriceRepository =
        new(() => new ProductPriceRepository(repositoryContext));

    private readonly Lazy<IProductRepository> _productRepository = new(() => new ProductRepository(repositoryContext));

    private readonly Lazy<ISettingRepository> _settingRepository = new(() => new SettingRepository(repositoryContext));

    private readonly Lazy<IUserLoginRepository> _userLoginRepository =
        new(() => new UserLoginRepository(repositoryContext));

    private readonly Lazy<IUserRepository> _userRepository = new(() => new UserRepository(repositoryContext));

    private readonly Lazy<IUserRoleRepository> _userRoleRepository =
        new(() => new UserRoleRepository(repositoryContext));

    public IUserLoginRepository UserLogin => _userLoginRepository.Value;
    public IUserRepository User => _userRepository.Value;
    public IUserRoleRepository UserRole => _userRoleRepository.Value;
    public ICustomerVendorRepository CustomerVendor => _customerVendorRepository.Value;
    public ISettingRepository Setting => _settingRepository.Value;

    public ICategoryRepository Category => _categoryRepository.Value;
    public IProductRepository Product => _productRepository.Value;
    public IProductPriceRepository ProductPrice => _productPriceRepository.Value;

    public IInvoicePurchaseRepository InvoicePurchase => _invoicePurchaseRepository.Value;
    public IInvoicePurchaseProductRepository InvoicePurchaseProduct => _invoicePurchaseProductRepository.Value;
    public IInvoiceSaleRepository InvoiceSale => _invoiceSaleRepository.Value;
    public IInvoiceSaleProductRepository InvoiceSaleProduct => _invoiceSaleProductRepository.Value;
    public IInvoiceSaleDeliveryRepository InvoiceSaleDelivery => _invoiceSaleDeliveryRepository.Value;

    public INotificationRepository Notification => _notificationRepository.Value;
    public INotificationUserRepository NotificationUser => _notificationUserRepository.Value;

    public async Task BeginTransaction()
    {
        await repositoryContext.Database.BeginTransactionAsync();
    }

    public async Task SaveAsync()
    {
        await repositoryContext.SaveChangesAsync();
    }
}