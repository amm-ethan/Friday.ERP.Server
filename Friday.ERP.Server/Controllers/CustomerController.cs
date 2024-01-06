using Friday.ERP.Core.IServices;
using Friday.ERP.Server.ActionFilters;
using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Friday.ERP.Server.Controllers;

[ApiController]
[Route("api/customer-management/customers")]
[Authorize]
public class CustomerController(IServiceManager service) : ControllerBase
{
    [HttpPost("", Name = "CreateCustomer")]
    public async Task<IActionResult> CreateCustomer(CustomerVendorCreateDto customerVendorCreateDto)
    {
        var customerToReturn =
            await service.CustomerVendorService.CreateCustomerVendor(customerVendorCreateDto,
                CustomerVendorTypeEnum.Customer);

        return Ok(customerToReturn);
    }

    [HttpGet("", Name = "GetAllCustomers")]
    public async Task<IActionResult> GetAllCustomers([FromQuery] CustomerVendorParameter customerVendorParameter)
    {
        var (customersToReturn, metaData) =
            await service.CustomerVendorService.GetAllCustomersVendors(customerVendorParameter,
                CustomerVendorTypeEnum.Customer);
        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(metaData);
        return Ok(customersToReturn);
    }

    [HttpGet("{guid:guid}", Name = "GetCustomerByGuid")]
    public async Task<IActionResult> GetCustomerByGuid(Guid guid)
    {
        var customerToReturn = await service.CustomerVendorService.GetCustomerVendorByGuid(guid);
        return Ok(customerToReturn);
    }

    [HttpPut("{guid:guid}", Name = "UpdateCustomer")]
    [ServiceFilter(typeof(GetCurrentUserGuidActionFilter))]
    public async Task<IActionResult> UpdateCustomer(Guid guid, CustomerVendorUpdateDto customerVendorUpdateDto)
    {
        var userGuid = HttpContext.Items["current_user_id"] as string;
        var customerToReturn =
            await service.CustomerVendorService.UpdateCustomerVendor(guid, customerVendorUpdateDto, userGuid!);
        return Ok(customerToReturn);
    }

    [HttpGet("search", Name = "SearchCustomers")]
    public async Task<IActionResult> SearchCustomers([FromQuery] string? searchTerm)
    {
        var customersToReturn =
            await service.CustomerVendorService.SearchCustomersVendors(searchTerm!, CustomerVendorTypeEnum.Customer);
        return Ok(customersToReturn);
    }
}