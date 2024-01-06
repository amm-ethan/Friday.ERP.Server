using Friday.ERP.Core.Data.Entities.CustomerVendorManagement;
using Friday.ERP.Core.Exceptions.BadRequest;
using Friday.ERP.Core.Exceptions.NotFound;
using Friday.ERP.Core.IRepositories;
using Friday.ERP.Core.IServices;
using Friday.ERP.Core.IServices.Entities;
using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.Enums;
using Friday.ERP.Shared.RequestFeatures;

namespace Friday.ERP.Infrastructure.Services.Entities;

internal sealed class CustomerVendorService(IRepositoryManager repository, ILoggerManager logger)
    : ICustomerVendorService
{
    public async Task<CustomerVendorViewDto> CreateCustomerVendor(CustomerVendorCreateDto customerVendorCreateDto,
        CustomerVendorTypeEnum customerVendorType)
    {
        var customerVendor = CustomerVendor.FromCustomerVendorCreateDto(customerVendorCreateDto);

        var existingCustomer = await repository.CustomerVendor.GetCustomerVendorByName(customerVendorCreateDto.Name,
            false);
        if (existingCustomer is not null)
            throw new DuplicateEntryBadRequestException(customerVendorCreateDto.Name, "Name", "VendorCustomer");

        if (customerVendorCreateDto.Email is not null)
        {
            var existingEmail = await repository.CustomerVendor.GetCustomerVendorByEmail(customerVendorCreateDto.Email,
                false);
            if (existingEmail is not null)
                throw new DuplicateEntryBadRequestException(customerVendorCreateDto.Email!, "Email", "VendorCustomer");
        }

        if (customerVendorCreateDto.Phone is not null)
        {
            var existingPhone = await repository.CustomerVendor.GetCustomerVendorByPhone(customerVendorCreateDto.Phone,
                false);
            if (existingPhone is not null)
                throw new DuplicateEntryBadRequestException(customerVendorCreateDto.Phone!, "Phone", "VendorCustomer");
        }

        var currentCustomerVendorCode =
            await repository.CustomerVendor.GetCurrentCustomerVendorCode(customerVendorType, false);
        var customerVendorCodeString = customerVendorType.ToString();
        var customerVendorCodeDigit = 0;
        if (currentCustomerVendorCode is not null)
        {
            var splitCurrentCustomerVendorCode = currentCustomerVendorCode.Split('-');
            customerVendorCodeString = splitCurrentCustomerVendorCode[0];
            customerVendorCodeDigit = int.Parse(splitCurrentCustomerVendorCode[1].TrimStart('0'));
        }

        customerVendorCodeDigit++;
        var nextCustomerVendorCodeDigit = customerVendorCodeDigit.ToString().PadLeft(7, '0');
        customerVendor.Code = $"{customerVendorCodeString}-{nextCustomerVendorCodeDigit}";

        customerVendor.CustomerVendorType = customerVendorType;

        repository.CustomerVendor.CreateCustomerVendor(customerVendor);
        await repository.SaveAsync();

        return await GetCustomerVendorByGuid(customerVendor.Guid);
    }

    public async Task<CustomerVendorViewDto> GetCustomerVendorByGuid(Guid customerVendorGuid)
    {
        var customerVendor = await ValidateCustomerVendor(customerVendorGuid);
        return ToCustomerVendorViewDto(customerVendor);
    }

    public async Task<(List<CustomerVendorViewDto>, MetaData metaData)> GetAllCustomersVendors(
        CustomerVendorParameter customerVendorParameter, CustomerVendorTypeEnum customerVendorType)
    {
        var createdCustomersVendors =
            await repository.CustomerVendor.GetAllCustomersVendors(customerVendorParameter, customerVendorType, false);
        return (createdCustomersVendors.Select(ToCustomerVendorViewDto).ToList(),
            metaData: createdCustomersVendors.MetaData);
    }

    public async Task<List<CustomerVendorViewDto>> SearchCustomersVendors(string searchTerm,
        CustomerVendorTypeEnum customerVendorType)
    {
        var createdCustomers =
            await repository.CustomerVendor.SearchCustomersVendors(searchTerm, customerVendorType, false);
        return createdCustomers.Select(ToCustomerVendorViewDto).ToList();
    }

    public async Task<CustomerVendorViewDto> UpdateCustomerVendor(Guid customerVendorGuid,
        CustomerVendorUpdateDto customerVendorUpdateDto, string currentUserGuid)
    {
        var currentCustomerVendor = await ValidateCustomerVendor(customerVendorGuid, true);

        if (customerVendorUpdateDto.Email is not null)
            currentCustomerVendor.Email = customerVendorUpdateDto.Email;

        if (customerVendorUpdateDto.Phone is not null)
            currentCustomerVendor.Phone = customerVendorUpdateDto.Phone;

        if (customerVendorUpdateDto.Address is not null)
            currentCustomerVendor.Address = customerVendorUpdateDto.Address;

        if (customerVendorUpdateDto.TotalCreditDebitLeft is not null)
            currentCustomerVendor.TotalCreditDebitLeft = customerVendorUpdateDto.TotalCreditDebitLeft ?? 0;

        await repository.SaveAsync();

        logger.LogInfo($"Customer {currentCustomerVendor.Guid} is Updated by UserId {currentUserGuid}");

        return await GetCustomerVendorByGuid(customerVendorGuid);
    }

    public async Task UpdateCustomerVendorCreditDebit(Guid customerVendorGuid, long totalCreditDebitLeft)
    {
        var currentCustomerVendor = await ValidateCustomerVendor(customerVendorGuid, true);
        currentCustomerVendor.TotalCreditDebitLeft += totalCreditDebitLeft;

        await repository.SaveAsync();
    }

    public async Task<CustomerDebitSummaryViewDto> GetTotalDebitsOfCustomers()
    {
        var totalDebits = await repository.CustomerVendor.GetTotalDebitsOfCustomers();
        var customersToReturn = await repository.CustomerVendor.GetAllCustomersWhoHasDebits();
        var customersWithDebit = customersToReturn.Select(ToFrequentCustomerDto).ToList();

        return new CustomerDebitSummaryViewDto(
            totalDebits,
            customersWithDebit
        );
    }

    #region Private Methods

    private static CustomerVendorViewDto ToCustomerVendorViewDto(CustomerVendor customerVendor)
    {
        return new CustomerVendorViewDto
        (
            customerVendor.Guid,
            customerVendor.Code!,
            customerVendor.Name!,
            Email: customerVendor.Email,
            Phone: customerVendor.Phone,
            Address: customerVendor.Address,
            TotalCreditDebitLeft: customerVendor.TotalCreditDebitLeft
        );
    }

    private static CustomerWithDebitViewDto ToFrequentCustomerDto(CustomerVendor customerVendor)
    {
        return new CustomerWithDebitViewDto
        (
            customerVendor.Guid,
            customerVendor.Name!,
            customerVendor.TotalCreditDebitLeft
        );
    }

    private async Task<CustomerVendor> ValidateCustomerVendor(Guid customerVendorGuid, bool trackChanges = false)
    {
        var customer = await repository.CustomerVendor.GetCustomerVendorByGuid(customerVendorGuid, trackChanges);
        if (customer is null)
            throw new ObjectNotFoundByFilterException("UserGuid", "User",
                customerVendorGuid.ToString());
        return customer;
    }

    #endregion
}