using Friday.ERP.Core.Data.Entities.InventoryManagement;
using Friday.ERP.Core.IRepositories.Entities.InventoryManagement;
using Friday.ERP.Infrastructure.Utilities.Entities;
using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Friday.ERP.Infrastructure.Repositories.Entities.InventoryManagement;

internal sealed class CategoryRepository(RepositoryContext repositoryContext)
    : RepositoryBase<Category>(repositoryContext), ICategoryRepository
{
    public async Task<Category?> GetCategoryByName(string name, bool trackChanges)
    {
        return await FindByCondition(c => c.Name == name && c.IsActive == true, trackChanges).SingleOrDefaultAsync();
    }

    public async Task<Category?> GetCategoryByGuid(Guid guid, bool trackChanges)
    {
        return await FindByCondition(c => c.Guid.Equals(guid) && c.IsActive == true, trackChanges)
            .Include(c => c.Products)
            .SingleOrDefaultAsync();
    }

    public async Task<PagedList<Category>> GetAllCategories(CategoryParameter categoryParameter, bool trackChanges)
    {
        var categories = await FindByCondition(c => c.IsActive == true, trackChanges)
            .Search(categoryParameter.SearchTerm)
            .Sort(categoryParameter.OrderBy!)
            .Skip((categoryParameter.PageNumber - 1) * categoryParameter.PageSize)
            .Take(categoryParameter.PageSize)
            .ToListAsync();

        var count = await FindByCondition(c => c.IsActive == true, trackChanges)
            .Search(categoryParameter.SearchTerm)
            .Sort(categoryParameter.OrderBy!).CountAsync();

        return PagedList<Category>.ToPagedList(categories, count, categoryParameter.PageNumber,
            categoryParameter.PageSize);
    }

    public async Task<IEnumerable<Category>> SearchAllCategories(string? searchTerm, bool trackChanges)
    {
        return await FindByCondition(c => c.IsActive == true, trackChanges)
            .Search(searchTerm)
            .Take(10)
            .ToListAsync();
    }

    public void CreateCategory(Category category)
    {
        Create(category);
    }
}