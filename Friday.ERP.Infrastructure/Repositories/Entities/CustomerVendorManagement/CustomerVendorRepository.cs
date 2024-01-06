using Friday.ERP.Core.Data.Entities.CustomerVendorManagement;
using Friday.ERP.Core.IRepositories.Entities.CustomerVendorManagement;
using Friday.ERP.Infrastructure.Utilities.Entities;
using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.Enums;
using Friday.ERP.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Friday.ERP.Infrastructure.Repositories.Entities.CustomerVendorManagement;

internal sealed class CustomerVendorRepository(RepositoryContext repositoryContext)
    : RepositoryBase<CustomerVendor>(repositoryContext), ICustomerVendorRepository
{
    public void CreateCustomerVendor(CustomerVendor customerVendor)
    {
        Create(customerVendor);
    }

    public async Task<CustomerVendor?> GetCustomerVendorByGuid(Guid guid, bool trackChanges)
    {
        return await FindByCondition(c => c.Guid.Equals(guid), trackChanges).SingleOrDefaultAsync();
    }

    public async Task<CustomerVendor?> GetCustomerVendorByName(string name, bool trackChanges)
    {
        return await FindByCondition(c => c.Name == name, trackChanges).SingleOrDefaultAsync();
    }

    public async Task<CustomerVendor?> GetCustomerVendorByEmail(string email, bool trackChanges)
    {
        return await FindByCondition(c => c.Email == email, trackChanges).SingleOrDefaultAsync();
    }

    public async Task<CustomerVendor?> GetCustomerVendorByPhone(string phone, bool trackChanges)
    {
        return await FindByCondition(c => c.Phone == phone, trackChanges).SingleOrDefaultAsync();
    }

    public async Task<PagedList<CustomerVendor>> GetAllCustomersVendors(CustomerVendorParameter customerVendorParameter,
        CustomerVendorTypeEnum customerVendorType, bool trackChanges)
    {
        var customersVendors = await FindAll(trackChanges)
            .Filter(customerVendorType)
            .Search(customerVendorParameter.SearchTerm)
            .Sort(customerVendorParameter.OrderBy!)
            .Skip((customerVendorParameter.PageNumber - 1) * customerVendorParameter.PageSize)
            .Take(customerVendorParameter.PageSize)
            .ToListAsync();

        var count = await FindAll(trackChanges)
            .Filter(customerVendorType)
            .Search(customerVendorParameter.SearchTerm)
            .Sort(customerVendorParameter.OrderBy!).CountAsync();

        return PagedList<CustomerVendor>.ToPagedList(customersVendors, count, customerVendorParameter.PageNumber,
            customerVendorParameter.PageSize);
    }

    public async Task<IEnumerable<CustomerVendor>> SearchCustomersVendors(string searchTerm,
        CustomerVendorTypeEnum customerVendorType, bool trackChanges)
    {
        return await FindAll(trackChanges)
            .Filter(customerVendorType)
            .Search(searchTerm)
            .Take(10)
            .ToListAsync();
    }

    public async Task<string?> GetCurrentCustomerVendorCode(CustomerVendorTypeEnum customerVendorType,
        bool trackChanges)
    {
        return await FindAll(trackChanges).OrderByDescending(c => c.Code)
            .Filter(customerVendorType)
            .Select(c => c.Code!)
            .FirstOrDefaultAsync();
    }

    public async Task<long> GetTotalDebitsOfCustomers()
    {
        return await FindByCondition(
                c => c.TotalCreditDebitLeft < 0 && c.CustomerVendorType == CustomerVendorTypeEnum.Customer, false)
            .SumAsync(c => c.TotalCreditDebitLeft);
    }

    public async Task<IEnumerable<CustomerVendor>> GetAllCustomersWhoHasDebits()
    {
        return await FindByCondition(
                c => c.TotalCreditDebitLeft < 0 && c.CustomerVendorType == CustomerVendorTypeEnum.Customer, false)
            .ToListAsync();
    }
}