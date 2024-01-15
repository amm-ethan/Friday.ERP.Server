using Friday.ERP.Shared.DataTransferObjects;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Friday.ERP.Server.Utilities.PdfGenerator.InvoiceSale;

public class Document(InvoiceSaleViewDto invoiceModel, SettingViewDto settingModel) : IDocument
{
    // private Image LogoImage { get; } = Image.FromBinaryData();

    public DocumentMetadata GetMetadata()
    {
        return DocumentMetadata.Default;
    }


    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Size(PageSizes.A5);
                page.Margin(20);
                page.DefaultTextStyle(x => x.FontFamily("Myanmar3"));

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().AlignCenter().Text(text =>
                {
                    text.CurrentPageNumber().FontSize(10);
                    text.Span(" / ").FontSize(10);
                    text.TotalPages().FontSize(10);
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
                    .Item().AlignCenter().Text($"{settingModel.ShopName}")
                    .FontSize(12).Bold();

                column
                    .Item().AlignCenter().Text($"{settingModel.ShopDescription}")
                    .FontSize(12);

                column
                    .Item().PaddingTop(10)
                    .AlignLeft().Text($"ဆိုင် - {settingModel.AddressOne}")
                    .FontSize(10);

                column
                    .Item().AlignLeft().Text($"အိမ် - {settingModel.AddressTwo}")
                    .FontSize(10);

                column
                    .Item().AlignLeft()
                    .Text(
                        $"ဖုန်း - {settingModel.PhoneOne}, {settingModel.PhoneTwo}, {settingModel.PhoneThree}, {settingModel.PhoneFour}")
                    .FontSize(10);

                column.Item().PaddingVertical(10).LineHorizontal(1);
            });
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(r =>
                    {
                        if (invoiceModel.Customer is not null)
                        {
                            r
                                .Item().Text($"အမည် - {invoiceModel.Customer!.Name}")
                                .FontSize(10);

                            r
                                .Item().Text($"လိပ်စာ - {invoiceModel.Customer!.Address}")
                                .FontSize(10);

                            r
                                .Item().Text($"ဖုန်း - {invoiceModel.Customer!.Phone}")
                                .FontSize(10);
                        }
                        else
                        {
                            r.Spacing(1);
                        }
                    });
                    row.RelativeItem().Column(r =>
                    {
                        r
                            .Item().AlignRight()
                            .Text($"ဘောင်ချာနံပါတ် - {invoiceModel.InvoiceNo}")
                            .FontSize(10);

                        r
                            .Item().AlignRight()
                            .Text($"နေ့စွဲ - {invoiceModel.PurchasedAt:MM/dd/yyyy}")
                            .FontSize(10);
                    });
                });

                column.Item().PaddingTop(15).Row(row =>
                {
                    row.RelativeItem().Component(new DocumentTable(invoiceModel.SoldProducts));
                });

                if (invoiceModel.Customer is not null)
                    column.Item().PaddingTop(15).AlignRight().Row(row =>
                    {
                        row.ConstantItem(100).AlignRight().Text("လုပ်သားခ").FontSize(10);
                        row.ConstantItem(100).AlignRight().Text($"{invoiceModel.DeliveryFees} ကျပ်").FontSize(10);
                    });

                column.Item().PaddingTop(15).AlignRight().Row(row =>
                {
                    row.ConstantItem(100).AlignRight().Text("စုစုပေါင်း").FontSize(10);
                    row.ConstantItem(100).AlignRight().Text($"{invoiceModel.Total} ကျပ်").FontSize(10);
                });

                if (invoiceModel.Customer is not null)
                {
                    column.Item().AlignRight().Row(row =>
                    {
                        row.ConstantItem(100).AlignRight().Text("ယခင်ကြွေးကျန်").FontSize(10);
                        row.ConstantItem(100).AlignRight().Text($"{invoiceModel.ExistingCreditDebit * -1} ကျပ်")
                            .FontSize(10);
                    });

                    column.Item().AlignRight().Row(row =>
                    {
                        row.ConstantItem(100).AlignRight().Text("ပေးရန်စုစုပေါင်း").FontSize(10);
                        row.ConstantItem(100).AlignRight().Text($"{invoiceModel.GrandTotal} ကျပ်").FontSize(10);
                    });
                }
                else
                {
                    column.Item().PaddingTop(15).AlignRight().Row(row =>
                    {
                        row.ConstantItem(100).AlignRight().Text("ပေးရန်စုစုပေါင်း").FontSize(10);
                        row.ConstantItem(100).AlignRight().Text($"{invoiceModel.Total} ကျပ်").FontSize(10);
                    });
                }

                column.Item().PaddingTop(15).AlignRight().Row(row =>
                {
                    row.ConstantItem(100).AlignRight().Text("ပေးငွေစုစုပေါင်း").FontSize(10);
                    row.ConstantItem(100).AlignRight().Text($"{invoiceModel.PaidTotal} ကျပ်").FontSize(10);
                });

                if (invoiceModel.Customer is not null)
                    column.Item().PaddingTop(15).AlignRight().Row(row =>
                    {
                        row.ConstantItem(100).AlignRight().Text("ကျန်ငွေစုစုပေါင်း").FontSize(10);
                        row.ConstantItem(100).AlignRight().Text($"{invoiceModel.CreditDebitLeft} ကျပ်").FontSize(10);
                    });

                if (invoiceModel.Remark is not null)
                    column.Item().PaddingTop(10).Row(row =>
                    {
                        row.RelativeItem().Component(new DocumentRemark(invoiceModel.Remark));
                    });
            }
        );
    }
}