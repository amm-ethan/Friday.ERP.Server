using System.Linq.Expressions;
using Friday.ERP.Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Friday.ERP.Infrastructure.Repositories;

public abstract class RepositoryBase<T>(RepositoryContext repositoryContext) : IRepositoryBase<T>
    where T : class
{
    public IQueryable<T> FindAll(bool trackChanges)
    {
        return !trackChanges ? repositoryContext.Set<T>().AsNoTracking() : repositoryContext.Set<T>();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges)
    {
        return !trackChanges
            ? repositoryContext.Set<T>()
                .Where(expression)
                .AsNoTracking()
            : repositoryContext.Set<T>()
                .Where(expression);
    }

    public void Create(T entity)
    {
        repositoryContext.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        repositoryContext.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        repositoryContext.Set<T>().Remove(entity);
    }
}