using System.Linq.Dynamic.Core;
using Friday.ERP.Core.Data.Entities.InvoiceManagement;
using NinjaNye.SearchExtensions;

namespace Friday.ERP.Infrastructure.Utilities.Entities;

public static class InvoiceSaleExtensions
{
    public static IQueryable<InvoiceSale> Filter(this IQueryable<InvoiceSale> invoices, Guid? vendorGuid,
        DateTime? fromDate,
        DateTime? toDate)
    {
        if (fromDate == null && toDate == null && vendorGuid == null)
            return invoices;
        if (fromDate != null && toDate != null && vendorGuid == null)
            return invoices.Where(x => x.PurchasedAt >= fromDate && x.PurchasedAt <= toDate);
        if (fromDate == null && toDate == null && vendorGuid != null)
            return invoices.Where(x => x.CustomerGuid == vendorGuid);
        if (fromDate != null && toDate != null && vendorGuid != null)
            return invoices.Where(x =>
                x.PurchasedAt >= fromDate && x.PurchasedAt <= toDate && x.CustomerGuid == vendorGuid);
        return invoices;
    }

    public static IQueryable<InvoiceSale> Search(this IQueryable<InvoiceSale> invoices, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return invoices;
        var lowerCaseTerm = searchTerm.Trim().ToLower();
        return invoices.Search(x => x.InvoiceNo!.ToLower()).Containing(lowerCaseTerm);
    }

    public static IQueryable<InvoiceSale> Sort(this IQueryable<InvoiceSale> transactions,
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