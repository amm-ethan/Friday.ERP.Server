using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Friday.ERP.Server.Utilities.PdfGenerator.InvoiceSale;

public class DocumentRemark(string comment) : IComponent
{
    public void Compose(IContainer container)
    {
        container.ShowEntire().Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
        {
            column.Item().Text("မှတ်ချက်").FontSize(10);
            column.Item().Text(comment).FontSize(10);
        });
    }
}