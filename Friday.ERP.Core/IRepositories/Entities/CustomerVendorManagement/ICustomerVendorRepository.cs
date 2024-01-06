using Friday.ERP.Core.Data.Entities.CustomerVendorManagement;
using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.Enums;
using Friday.ERP.Shared.RequestFeatures;

namespace Friday.ERP.Core.IRepositories.Entities.CustomerVendorManagement;

public interface ICustomerVendorRepository
{
    void CreateCustomerVendor(CustomerVendor customerVendor);

    Task<CustomerVendor?> GetCustomerVendorByGuid(Guid guid, bool trackChanges);

    Task<CustomerVendor?> GetCustomerVendorByName(string name, bool trackChanges);

    Task<CustomerVendor?> GetCustomerVendorByEmail(string email, bool trackChanges);

    Task<CustomerVendor?> GetCustomerVendorByPhone(string phone, bool trackChanges);

    Task<PagedList<CustomerVendor>> GetAllCustomersVendors(CustomerVendorParameter customerVendorParameter,
        CustomerVendorTypeEnum customerVendorType, bool trackChanges);

    Task<IEnumerable<CustomerVendor>> SearchCustomersVendors(string searchTerm,
        CustomerVendorTypeEnum customerVendorType,
        bool trackChanges);

    Task<string?> GetCurrentCustomerVendorCode(CustomerVendorTypeEnum customerVendorType, bool trackChanges);


    Task<long> GetTotalDebitsOfCustomers();

    Task<IEnumerable<CustomerVendor>> GetAllCustomersWhoHasDebits();
}