using System.Linq.Dynamic.Core;
using Friday.ERP.Core.Data.Entities.InventoryManagement;
using NinjaNye.SearchExtensions;

namespace Friday.ERP.Infrastructure.Utilities.Entities;

public static class CategoryExtension
{
    public static IQueryable<Category> Search(this IQueryable<Category> categories, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return categories;
        var lowerCaseTerm = searchTerm.Trim().ToLower();
        return categories.Search(x => x.Name!.ToLower()).Containing(lowerCaseTerm);
    }

    public static IQueryable<Category> Sort(this IQueryable<Category> categories, string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return categories.OrderBy(e => e.Name);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Category>(orderByQueryString);

        return string.IsNullOrWhiteSpace(orderQuery)
            ? categories.OrderBy(e => e.Name)
            : categories.OrderBy(orderQuery);
    }
}