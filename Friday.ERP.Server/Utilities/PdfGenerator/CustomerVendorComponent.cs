using Friday.ERP.Shared.DataTransferObjects;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Friday.ERP.Server.Utilities.PdfGenerator;

public class CustomerVendorComponent(string title, CustomerVendorViewDto model) : IComponent
{
    public void Compose(IContainer container)
    {
        container.ShowEntire().Column(column =>
        {
            column.Spacing(2);

            column.Item().Text(title).SemiBold();
            column.Item().PaddingBottom(5).LineHorizontal(1);

            column.Item().Row(row =>
            {
                row.ConstantItem(75).Text("Name:");
                row.RelativeItem().Text(model.Name);
            });
            column.Item().Row(row =>
            {
                row.ConstantItem(75).Text("Phone:");
                row.RelativeItem().Text(model.Phone);
            });
            column.Item().Row(row =>
            {
                row.ConstantItem(75).Text("Email:");
                row.RelativeItem().Text(model.Email);
            });
            column.Item().Row(row =>
            {
                row.ConstantItem(75).Text("Address:");
                row.RelativeItem().Text(model.Address);
            });
        });
    }
}