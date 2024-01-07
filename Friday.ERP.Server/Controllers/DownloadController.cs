using Friday.ERP.Core.IServices;
using Friday.ERP.Server.Utilities.PdfGenerator.InvoiceSale;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using Document = Friday.ERP.Server.Utilities.PdfGenerator.InvoiceSale.Document;

namespace Friday.ERP.Server.Controllers;

[ApiController]
[Route("api/download/invoices")]
[Authorize]
public class DownloadController(IServiceManager service, IWebHostEnvironment env) : ControllerBase
{
    private readonly string _product_images = Path.Combine(env.WebRootPath, "product_images");

    [HttpGet("sales/{guid:guid}", Name = "DownloadSaleInvoice")]
    public async Task<FileStreamResult> DownloadSaleInvoice(Guid guid)
    {
        var invoiceSaleViewDto = await service.InvoiceService.GetInvoiceSaleByGuid(guid, _product_images);
        var settingViewDto = await service.SystemService.GetSettings(env.WebRootPath);

        var document = new Document(invoiceSaleViewDto, settingViewDto);

        var pdfBytes = document.GeneratePdf();
        var ms = new MemoryStream(pdfBytes);
        var filename = $"{invoiceSaleViewDto.InvoiceNo}_{Guid.NewGuid():N}.pdf";
        return File(ms, "application/pdf", filename);
    }
}