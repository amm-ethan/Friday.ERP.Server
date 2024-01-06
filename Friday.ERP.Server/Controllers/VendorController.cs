using Friday.ERP.Core.IServices;
using Friday.ERP.Server.ActionFilters;
using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Friday.ERP.Server.Controllers;

[ApiController]
[Route("api/vendor-management/vendors")]
[Authorize]
public class VendorController(IServiceManager service) : ControllerBase
{
    [HttpPost("", Name = "CreateVendor")]
    public async Task<IActionResult> CreateVendor(CustomerVendorCreateDto customerVendorCreateDto)
    {
        var vendorToReturn =
            await service.CustomerVendorService.CreateCustomerVendor(customerVendorCreateDto,
                CustomerVendorTypeEnum.Vendor);

        return Ok(vendorToReturn);
    }

    [HttpGet("", Name = "GetAllVendors")]
    public async Task<IActionResult> GetAllVendors([FromQuery] CustomerVendorParameter customerVendorParameter)
    {
        var (vendorsToReturn, metaData) =
            await service.CustomerVendorService.GetAllCustomersVendors(customerVendorParameter,
                CustomerVendorTypeEnum.Vendor);
        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(metaData);
        return Ok(vendorsToReturn);
    }

    [HttpGet("{guid:guid}", Name = "GetVendorByGuid")]
    public async Task<IActionResult> GetVendorByGuid(Guid guid)
    {
        var vendorToReturn = await service.CustomerVendorService.GetCustomerVendorByGuid(guid);
        return Ok(vendorToReturn);
    }

    [HttpPut("{guid:guid}", Name = "UpdateVendor")]
    [ServiceFilter(typeof(GetCurrentUserGuidActionFilter))]
    public async Task<IActionResult> UpdateVendor(Guid guid, CustomerVendorUpdateDto customerVendorUpdateDto)
    {
        var userGuid = HttpContext.Items["current_user_id"] as string;
        var vendorToReturn =
            await service.CustomerVendorService.UpdateCustomerVendor(guid, customerVendorUpdateDto, userGuid!);
        return Ok(vendorToReturn);
    }

    [HttpGet("search", Name = "SearchVendors")]
    public async Task<IActionResult> SearchVendors([FromQuery] string? searchTerm)
    {
        var vendorsToReturn =
            await service.CustomerVendorService.SearchCustomersVendors(searchTerm!, CustomerVendorTypeEnum.Vendor);
        return Ok(vendorsToReturn);
    }
}