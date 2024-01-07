using Friday.ERP.Shared.DataTransferObjects;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Friday.ERP.Server.Utilities.PdfGenerator.InvoiceSale;

public class DocumentTable(List<InvoiceSaleProductViewDto> models) : IComponent
{
    public void Compose(IContainer container)
    {
        var style = TextStyle.Default.SemiBold().FontSize(8);
        static IContainer CellStyle(IContainer container)
        {
            return container.Border(1).BorderColor(Colors.Grey.Darken1).PaddingVertical(5);
        }
        
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(25);
                columns.RelativeColumn(3);
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).AlignCenter().Text("#");
                header.Cell().Element(CellStyle).AlignCenter().Text("အမျိုးအမည်").Style(style);
                header.Cell().Element(CellStyle).AlignCenter().Text("ရေတွက်ပုံ").Style(style);
                header.Cell().Element(CellStyle).AlignCenter().Text("နှုန်း").Style(style);
                header.Cell().Element(CellStyle).AlignCenter().Text("သင့်ငွေ").Style(style);
            });

            foreach (var item in models)
            {
                var index = models.IndexOf(item) + 1;

                table.Cell().Element(CellStyle).AlignCenter().Text($"{index}").Style(style);
                table.Cell().Element(CellStyle).AlignCenter().Text(item.ProductName).Style(style);
                table.Cell().Element(CellStyle).AlignCenter().Text($"{item.Quantity} ခု").Style(style);
                table.Cell().Element(CellStyle).AlignCenter().Text($"{item.ProducePriceSalePrice}").Style(style);
                table.Cell().Element(CellStyle).AlignCenter().Text($"{item.TotalPrice}").Style(style);
            }
        });
    }
}