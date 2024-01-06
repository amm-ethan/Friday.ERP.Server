using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Friday.ERP.Server.Utilities.PdfGenerator;

public class CommentComponent(string comment) : IComponent
{
    public void Compose(IContainer container)
    {
        container.ShowEntire().Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
        {
            column.Spacing(5);
            column.Item().Text("Remark").FontSize(14).SemiBold();
            column.Item().Text(comment);
        });
    }
}