using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.Enums;
using Friday.ERP.Shared.RequestFeatures;

namespace Friday.ERP.Core.IServices.Entities;

public interface ICustomerVendorService
{
    Task<CustomerVendorViewDto> CreateCustomerVendor(CustomerVendorCreateDto customerVendorCreateDto,
        CustomerVendorTypeEnum customerVendorType);

    Task<CustomerVendorViewDto> GetCustomerVendorByGuid(Guid customerVendorGuid);

    Task<(List<CustomerVendorViewDto>, MetaData metaData)> GetAllCustomersVendors(
        CustomerVendorParameter customerVendorParameter, CustomerVendorTypeEnum customerVendorType);

    Task<List<CustomerVendorViewDto>> SearchCustomersVendors(string searchTerm,
        CustomerVendorTypeEnum customerVendorType);

    Task<CustomerVendorViewDto> UpdateCustomerVendor(Guid customerVendorGuid,
        CustomerVendorUpdateDto customerVendorUpdateDto,
        string currentUserGuid);

    Task UpdateCustomerVendorCreditDebit(Guid customerVendorGuid, long totalCreditDebitLeft);

    Task<CustomerDebitSummaryViewDto> GetTotalDebitsOfCustomers();
}