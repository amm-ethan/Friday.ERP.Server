using System.Linq.Dynamic.Core;
using Friday.ERP.Core.Data.Entities.CustomerVendorManagement;
using Friday.ERP.Shared.Enums;
using NinjaNye.SearchExtensions;

namespace Friday.ERP.Infrastructure.Utilities.Entities;

public static class CustomerVendorExtensions
{
    public static IQueryable<CustomerVendor> Filter(this IQueryable<CustomerVendor> customersVendors,
        CustomerVendorTypeEnum customerVendorType)
    {
        return customersVendors.Where(c => c.CustomerVendorType == customerVendorType);
    }

    public static IQueryable<CustomerVendor> Search(this IQueryable<CustomerVendor> customersVendors,
        string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return customersVendors;
        var lowerCaseTerm = searchTerm.Trim().ToLower();
        return customersVendors.Search(x => x.Code!.ToLower(),
            x => x.Name!.ToLower(),
            x => x.Email!.ToLower(),
            x => x.Phone!.ToLower()
        ).Containing(lowerCaseTerm);
    }

    public static IQueryable<CustomerVendor> Sort(this IQueryable<CustomerVendor> customersVendors,
        string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return customersVendors.OrderBy(e => e.Code);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<CustomerVendor>(orderByQueryString);

        return string.IsNullOrWhiteSpace(orderQuery)
            ? customersVendors.OrderBy(e => e.Code)
            : customersVendors.OrderBy(orderQuery);
    }
}