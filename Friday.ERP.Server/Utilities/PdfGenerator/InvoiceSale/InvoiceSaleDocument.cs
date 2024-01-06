using Friday.ERP.Shared.DataTransferObjects;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Friday.ERP.Server.Utilities.PdfGenerator.InvoiceSale;

public class InvoiceSaleDocument : IDocument
{
    public InvoiceSaleDocument(InvoiceSaleViewDto model, IWebHostEnvironment env)
    {
        var imagePath = Path.Combine(env.WebRootPath, "logo.png");

        Model = model;
        LogoImage = Image.FromFile(imagePath);
    }

    private InvoiceSaleViewDto Model { get; }
    private Image LogoImage { get; }

    public DocumentMetadata GetMetadata()
    {
        return DocumentMetadata.Default;
    }


    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(50);

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);

                page.Footer().AlignCenter().Text(text =>
                {
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                });
            });
    }

    private void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column
                    .Item().Text($"Invoice #{Model.InvoiceNo}")
                    .FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                column.Item().Text(text =>
                {
                    text.Span("Purchased date: ").SemiBold();
                    text.Span($"{Model.PurchasedAt:d}");
                });
            });

            row.ConstantItem(150).Image(LogoImage);
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.PaddingVertical(40).Column(column =>
        {
            column.Spacing(20);

            if (Model.Customer is not null)
                column.Item().Row(row =>
                {
                    row.RelativeItem().Component(new CustomerVendorComponent("Customer Info", Model.Customer));
                });

            column.Item().Row(row =>
            {
                row.RelativeItem().Component(new InvoiceSaleProductTable(Model.SoldProducts));
            });
            column.Item().AlignRight().Row(row =>
            {
                row.ConstantItem(100).AlignRight().Text("SubTotal:").SemiBold();
                row.ConstantItem(100).AlignRight().Text($"{Model.SubTotal} mmk").SemiBold();
            });
            if (Model.Discount is not null)
                column.Item().AlignRight().Row(row =>
                {
                    row.ConstantItem(100).AlignRight().Text("Discount:").SemiBold();
                    row.ConstantItem(100).AlignRight().Text($"{Model.Discount}").SemiBold();
                });
            if (Model.DiscountType is not null)
                column.Item().AlignRight().Row(row =>
                {
                    row.ConstantItem(100).AlignRight().Text("Discount Type:").SemiBold();
                    row.ConstantItem(100).AlignRight().Text($"{Model.DiscountType!.ToString()}").SemiBold();
                });
            column.Item().AlignRight().Row(row =>
            {
                row.ConstantItem(100).AlignRight().Text("Grand Total:").SemiBold();
                row.ConstantItem(100).AlignRight().Text($"{Model.Total} mmk").SemiBold();
            });
            column.Item().AlignRight().Row(row =>
            {
                row.ConstantItem(100).AlignRight().Text("Paid Total:").SemiBold();
                row.ConstantItem(100).AlignRight().Text($"{Model.PaidTotal} mmk").SemiBold();
            });
            column.Item().AlignRight().Row(row =>
            {
                row.ConstantItem(100).AlignRight().Text("Credit Debit Left:").SemiBold();
                row.ConstantItem(100).AlignRight().Text($"{Model.CreditDebitLeft} mmk").SemiBold();
            });

            if (Model.DeliveryInfo is not null)
                column.Item().Row(row =>
                {
                    row.RelativeItem().Component(new InvoiceSaleDelivery(Model.DeliveryInfo));
                });

            column.Item().Row(row => { row.RelativeItem().Component(new CommentComponent("Comment")); });
        });
    }
}