using System.Linq.Dynamic.Core;
using Friday.ERP.Core.Data.Entities.AccountManagement;
using NinjaNye.SearchExtensions;

namespace Friday.ERP.Infrastructure.Utilities.Entities;

public static class UserExtensions
{
    public static IQueryable<User> Search(this IQueryable<User> users, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return users;
        var lowerCaseTerm = searchTerm.Trim().ToLower();
        return users.Search(x => x.Name!.ToLower(),
            x => x.Email!.ToLower(),
            x => x.Username!.ToLower()).Containing(lowerCaseTerm);
    }

    public static IQueryable<User> Sort(this IQueryable<User> users, string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return users.OrderBy(e => e.Name);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<User>(orderByQueryString);

        return string.IsNullOrWhiteSpace(orderQuery)
            ? users.OrderBy(e => e.Name)
            : users.OrderBy(orderQuery);
    }
}