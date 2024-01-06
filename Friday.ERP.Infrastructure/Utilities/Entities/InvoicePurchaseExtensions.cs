using System.Linq.Dynamic.Core;
using Friday.ERP.Core.Data.Entities.InvoiceManagement;
using NinjaNye.SearchExtensions;

namespace Friday.ERP.Infrastructure.Utilities.Entities;

public static class InvoicePurchaseExtensions
{
    public static IQueryable<InvoicePurchase> Filter(this IQueryable<InvoicePurchase> invoices, Guid? vendorGuid,
        DateTime? fromDate,
        DateTime? toDate)
    {
        if (fromDate == null && toDate == null && vendorGuid == null)
            return invoices;
        if (fromDate != null && toDate != null && vendorGuid == null)
            return invoices.Where(x => x.PurchasedAt >= fromDate && x.PurchasedAt <= toDate);
        if (fromDate == null && toDate == null && vendorGuid != null)
            return invoices.Where(x => x.VendorGuid == vendorGuid);
        if (fromDate != null && toDate != null && vendorGuid != null)
            return invoices.Where(x =>
                x.PurchasedAt >= fromDate && x.PurchasedAt <= toDate && x.VendorGuid == vendorGuid);
        return invoices;
    }

    public static IQueryable<InvoicePurchase> Search(this IQueryable<InvoicePurchase> invoices, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return invoices;
        var lowerCaseTerm = searchTerm.Trim().ToLower();
        return invoices.Search(x => x.InvoiceNo!.ToLower()).Containing(lowerCaseTerm);
    }

    public static IQueryable<InvoicePurchase> Sort(this IQueryable<InvoicePurchase> transactions,
        string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return transactions.OrderBy(e => e.InvoiceNo);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<InvoicePurchase>(orderByQueryString);

        return string.IsNullOrWhiteSpace(orderQuery)
            ? transactions.OrderBy(e => e.InvoiceNo)
            : transactions.OrderBy(orderQuery);
    }
}