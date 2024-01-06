using Friday.ERP.Shared.DataTransferObjects;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Friday.ERP.Server.Utilities.PdfGenerator.InvoiceSale;

public class InvoiceSaleDelivery(InvoiceSaleDeliveryViewDto model) : IComponent
{
    public void Compose(IContainer container)
    {
        container.ShowEntire().Column(column =>
        {
            column.Spacing(2);

            column.Item().Text("Delivery Info").SemiBold();
            column.Item().PaddingBottom(5).LineHorizontal(1);

            column.Item().Row(row =>
            {
                row.ConstantItem(150).Text("Address:");
                row.RelativeItem().Text(model.Address);
            });
            column.Item().Row(row =>
            {
                row.ConstantItem(150).Text("Contact Person:");
                row.RelativeItem().Text(model.ContactPerson);
            });
            column.Item().Row(row =>
            {
                row.ConstantItem(150).Text("Contact Phone:");
                row.RelativeItem().Text(model.ContactPhone);
            });
            if (model.DeliveryServiceName is not null)
                column.Item().Row(row =>
                {
                    row.ConstantItem(150).Text("Delivery Service Name:");
                    row.RelativeItem().Text(model.DeliveryServiceName);
                });
            if (model.DeliveryContactPerson is not null)
                column.Item().Row(row =>
                {
                    row.ConstantItem(150).Text("Delivery Contact Person:");
                    row.RelativeItem().Text(model.DeliveryContactPerson);
                });
            if (model.DeliveryContactPhone is not null)
                column.Item().Row(row =>
                {
                    row.ConstantItem(150).Text("Delivery Contact Phone:");
                    row.RelativeItem().Text(model.DeliveryContactPhone);
                });
            if (model.Remark is not null)
                column.Item().Row(row =>
                {
                    row.ConstantItem(150).Text("Remark:");
                    row.RelativeItem().Text(model.Remark);
                });
            if (model.DeliveryFees is not null)
                column.Item().Row(row =>
                {
                    row.ConstantItem(150).Text("Delivery Fees:");
                    row.RelativeItem().Text(model.DeliveryFees.ToString());
                });
        });
    }
}