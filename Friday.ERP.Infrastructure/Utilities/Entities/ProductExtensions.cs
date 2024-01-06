using System.Linq.Dynamic.Core;
using Friday.ERP.Core.Data.Entities.InventoryManagement;
using NinjaNye.SearchExtensions;

namespace Friday.ERP.Infrastructure.Utilities.Entities;

public static class ProductExtensions
{
    public static IQueryable<Product> Filter(this IQueryable<Product> products, Guid? guid)
    {
        return guid != null ? products.Where(c => c.CategoryGuid == guid) : products;
    }

    public static IQueryable<Product> Search(this IQueryable<Product> products, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return products;
        var lowerCaseTerm = searchTerm.Trim().ToLower();
        return products.Search(x => x.Name!.ToLower(),
            x => x.Code!.ToLower()).Containing(lowerCaseTerm);
    }

    public static IQueryable<Product> Sort(this IQueryable<Product> products, string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return products.OrderBy(e => e.Name);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Category>(orderByQueryString);

        return string.IsNullOrWhiteSpace(orderQuery)
            ? products.OrderBy(e => e.Name)
            : products.OrderBy(orderQuery);
    }
}