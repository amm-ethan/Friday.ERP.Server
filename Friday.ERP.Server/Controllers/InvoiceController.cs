using Friday.ERP.Core.IServices;
using Friday.ERP.Core.IServices.Hubs;
using Friday.ERP.Server.ActionFilters;
using Friday.ERP.Server.Hubs;
using Friday.ERP.Server.Utilities.PdfGenerator.InvoiceSale;
using Friday.ERP.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using QuestPDF.Fluent;

namespace Friday.ERP.Server.Controllers;

[ApiController]
[Route("api/invoice-management")]
[Authorize]
public class InvoiceController(
    IServiceManager service,
    IWebHostEnvironment env,
    IHubContext<NotificationHub> notificationHubContext,
    IUserConnectionManager userConnectionManager
)
    : ControllerBase
{
    private readonly string _wwwroot = Path.Combine(env.WebRootPath, "product_images");

    [HttpPost("sales", Name = "CreateSaleInvoice")]
    [ServiceFilter(typeof(GetCurrentUserGuidActionFilter))]
    public async Task<IActionResult> CreateSaleInvoice(InvoiceSaleCreateDto invoiceSaleCreateDto)
    {
        var userGuid = HttpContext.Items["current_user_id"] as string;
        var invoiceSaleViewDto = await service.InvoiceService.CreateInvoiceSale(invoiceSaleCreateDto, _wwwroot);

        if (invoiceSaleCreateDto.CustomerGuid is not null)
            await service.CustomerVendorService.UpdateCustomerVendorCreditDebit(
                invoiceSaleCreateDto.CustomerGuid ?? Guid.Empty,
                invoiceSaleCreateDto.CreditDebitLeft);

        foreach (var soldProduct in invoiceSaleCreateDto.SoldProducts)
        {
            var currentProductInfo =
                await service.InventoryService.GetProductNameAndStockByGuid(soldProduct.ProductGuid);
            var settings = await service.SystemService.GetSettings(null);
            if (currentProductInfo.stock > settings.MinimumStockMargin) continue;
            var notificationToCreate = new NotificationCreateDto
            (
                "Stock Low",
                $"{currentProductInfo.productName} only have {currentProductInfo.stock} stocks left"
            );

            var notificationGuid =
                await service.SystemService.CreateNotification(notificationToCreate, true, []);

            var notificationToSend = await service.SystemService.GetNotificationByGuid(
                notificationGuid, Guid.Parse(userGuid!));

            var connections = userConnectionManager.GetAllActiveUserConnections();
            if (connections.Count <= 0) continue;
            foreach (var connectionId in connections)
                await notificationHubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification",
                    JsonConvert.SerializeObject(notificationToSend));
        }

        var document = new InvoiceSaleDocument(invoiceSaleViewDto, env);

        var pdfBytes = document.GeneratePdf();
        var ms = new MemoryStream(pdfBytes);
        var filename = $"{invoiceSaleViewDto.InvoiceNo}_{Guid.NewGuid():N}.pdf";
        return File(ms, "application/pdf", filename);
    }

    [HttpGet("sales/{guid:guid}", Name = "GetSaleInvoiceByGuid")]
    public async Task<IActionResult> GetSaleInvoiceByGuid(Guid guid)
    {
        var dataToReturn =
            await service.InvoiceService.GetInvoiceSaleByGuid(guid, _wwwroot);
        return Ok(dataToReturn);
    }

    [HttpPut("sales/{guid:guid}", Name = "UpdateSaleInvoice")]
    [ServiceFilter(typeof(GetCurrentUserGuidActionFilter))]
    public async Task<IActionResult> UpdateSaleInvoice(Guid guid, InvoiceSaleUpdateDto invoiceSaleUpdateDto)
    {
        var userGuid = HttpContext.Items["current_user_id"] as string;
        var dataToReturn =
            await service.InvoiceService.UpdateInvoiceSale(guid, invoiceSaleUpdateDto, _wwwroot, userGuid!);
        return Ok(dataToReturn);
    }

    [HttpPut("sales/{guid:guid}/delivery", Name = "UpdateSaleInvoiceDelivery")]
    [ServiceFilter(typeof(GetCurrentUserGuidActionFilter))]
    public async Task<IActionResult> UpdateSaleInvoiceDelivery(Guid guid,
        InvoiceSaleDeliveryUpdateDto invoiceSaleDeliveryUpdateDto)
    {
        var userGuid = HttpContext.Items["current_user_id"] as string;
        var dataToReturn =
            await service.InvoiceService.UpdateInvoiceSaleDeliveryByGuid(guid, invoiceSaleDeliveryUpdateDto, userGuid!);
        return Ok(dataToReturn);
    }

    [HttpGet("sales", Name = "GetAllSalesInvoices")]
    public async Task<IActionResult> GetAllSalesInvoices([FromQuery] InvoiceParameter invoiceParameter)
    {
        var (invoicesToReturn, metaData) =
            await service.InvoiceService.GetAllInvoiceSales(invoiceParameter);
        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(metaData);
        return Ok(invoicesToReturn);
    }

    [HttpGet("sales/next-invoice-no", Name = "GetNextSaleInvoiceNumber")]
    public async Task<IActionResult> GetNextSaleInvoiceNumber()
    {
        var invoiceNumber =
            await service.InvoiceService.GetNextPurchaseInvoiceNumber(true);
        return Ok(invoiceNumber);
    }


    [HttpGet("purchases", Name = "GetAllPurchasesInvoices")]
    public async Task<IActionResult> GetAllPurchasesInvoices([FromQuery] InvoiceParameter invoiceParameter)
    {
        var (invoicesToReturn, metaData) =
            await service.InvoiceService.GetAllInvoicePurchases(invoiceParameter);
        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(metaData);
        return Ok(invoicesToReturn);
    }

    [HttpGet("purchases/next-invoice-no", Name = "GetNextPurchaseInvoiceNumber")]
    public async Task<IActionResult> GetNextPurchaseInvoiceNumber()
    {
        var invoiceNumber =
            await service.InvoiceService.GetNextPurchaseInvoiceNumber(false);
        return Ok(invoiceNumber);
    }

    [HttpPost("purchases", Name = "CreatePurchaseInvoice")]
    public async Task<IActionResult> CreatePurchaseInvoice(InvoicePurchaseCreateDto invoicePurchaseCreateDto)
    {
        var saleInvoiceToReturn =
            await service.InvoiceService.CreateInvoicePurchase(invoicePurchaseCreateDto, _wwwroot);
        await service.CustomerVendorService.UpdateCustomerVendorCreditDebit(
            invoicePurchaseCreateDto.VendorGuid,
            invoicePurchaseCreateDto.CreditDebitLeft);
        return Ok(saleInvoiceToReturn);
    }

    [HttpGet("purchases/{guid:guid}", Name = "GetPurchaseInvoiceByGuid")]
    public async Task<IActionResult> GetPurchaseInvoiceByGuid(Guid guid)
    {
        var dataToReturn =
            await service.InvoiceService.GetInvoicePurchaseByGuid(guid, _wwwroot);
        return Ok(dataToReturn);
    }


    [HttpPut("purchases/{guid:guid}", Name = "UpdatePurchaseInvoiceByGuid")]
    [ServiceFilter(typeof(GetCurrentUserGuidActionFilter))]
    public async Task<IActionResult> UpdatePurchaseInvoiceByGuid(Guid guid,
        InvoicePurchaseUpdateDto invoicePurchaseUpdateDto)
    {
        var userGuid = HttpContext.Items["current_user_id"] as string;
        var dataToReturn =
            await service.InvoiceService.UpdateInvoicePurchase(guid, invoicePurchaseUpdateDto, _wwwroot, userGuid!);
        return Ok(dataToReturn);
    }

    [HttpGet("purchases/product/{guid:guid}/price", Name = "GetLastPurchasedPriceOfProduct")]
    public async Task<IActionResult> GetLastPurchasedPriceOfProduct(Guid guid)
    {
        var dataToReturn =
            await service.InvoiceService.GetLastPurchasedPriceOfProduct(guid);
        return Ok(dataToReturn);
    }
}