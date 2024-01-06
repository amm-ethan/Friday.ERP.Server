using Friday.ERP.Core.Data.Constants.AppSettings;
using Friday.ERP.Core.IRepositories;
using Friday.ERP.Core.IServices;
using Friday.ERP.Core.IServices.Entities;
using Friday.ERP.Infrastructure.Services.Entities;
using Microsoft.Extensions.Options;

namespace Friday.ERP.Infrastructure.Services;

public sealed class ServiceManager(
    IRepositoryManager repositoryManager,
    ILoggerManager logger,
    IOptionsMonitor<JwtConfiguration> jwtConfiguration)
    : IServiceManager
{
    private readonly Lazy<IAccountService> _accountService = new(() =>
        new AccountService(repositoryManager, logger));

    private readonly Lazy<IAuthenticationService> _authenticationService = new(() =>
        new AuthenticationService(repositoryManager, logger, jwtConfiguration));

    private readonly Lazy<ICustomerVendorService> _customerService = new(() =>
        new CustomerVendorService(repositoryManager, logger));

    private readonly Lazy<IInventoryService> _inventoryService = new(() =>
        new InventoryService(repositoryManager, logger));

    private readonly Lazy<IInvoiceService> _invoiceService = new(() =>
        new InvoiceService(repositoryManager, logger));

    private readonly Lazy<ISystemService> _systemService = new(() =>
        new SystemService(repositoryManager, logger));

    public IAccountService AccountService => _accountService.Value;
    public IAuthenticationService AuthenticationService => _authenticationService.Value;
    public ICustomerVendorService CustomerVendorService => _customerService.Value;
    public IInventoryService InventoryService => _inventoryService.Value;
    public IInvoiceService InvoiceService => _invoiceService.Value;
    public ISystemService SystemService => _systemService.Value;
}