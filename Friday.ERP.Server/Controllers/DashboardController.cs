using Friday.ERP.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Friday.ERP.Server.Controllers;

[ApiController]
[Route("api/dashboard-management")]
[Authorize]
public class DashboardController(IServiceManager service) : ControllerBase
{
    private static readonly DateTime Today = DateTime.Today.AddDays(1);
    private readonly DateTime _requiredDate = Today.AddDays(-180);

    [HttpGet("customers-with-debit", Name = "GetCustomersWithDebit")]
    public async Task<IActionResult> GetCustomersWithDebit()
    {
        var dataToReturn = await service.CustomerVendorService.GetTotalDebitsOfCustomers();
        return Ok(dataToReturn);
    }

    [HttpGet("frequent-customers", Name = "GetFrequentCustomers")]
    public async Task<IActionResult> GetFrequentCustomers()
    {
        var dataToReturn = await service.InvoiceService.GetFrequentCustomersFromInvoiceSale(_requiredDate);
        return Ok(dataToReturn);
    }

    [HttpGet("top-selling-products", Name = "GetTopSellingProducts")]
    public async Task<IActionResult> GetTopSellingProducts()
    {
        var dataToReturn = await service.InvoiceService.GetTopSellingProductsFromInvoiceSale(_requiredDate);
        return Ok(dataToReturn);
    }

    [HttpGet("monthly-summary", Name = "GetMonthlySummary")]
    public async Task<IActionResult> GetMonthlySummary()
    {
        var dataToReturn = await service.InvoiceService.GetMonthlySummary(6);
        return Ok(dataToReturn);
    }
}