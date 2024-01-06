using Friday.ERP.Core.Data.Entities.InventoryManagement;
using Friday.ERP.Core.IRepositories.Entities.InventoryManagement;
using Friday.ERP.Infrastructure.Utilities.Entities;
using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Friday.ERP.Infrastructure.Repositories.Entities.InventoryManagement;

internal sealed class ProductRepository(RepositoryContext repositoryContext)
    : RepositoryBase<Product>(repositoryContext), IProductRepository
{
    public void CreateProduct(Product product)
    {
        Create(product);
    }

    public async Task<Product?> GetProductByName(string name, bool trackChanges)
    {
        return await FindByCondition(c => c.Name == name && c.IsActive, trackChanges).SingleOrDefaultAsync();
    }

    public async Task<Product?> GetProductByGuid(Guid guid, bool trackChanges)
    {
        return await FindByCondition(c => c.Guid.Equals(guid) && c.IsActive, trackChanges)
            .Include(c => c.Category)
            .Include(c => c.ProductPrices!.Where(x => x.IsActive))
            .SingleOrDefaultAsync();
    }

    public async Task<PagedList<Product>> GetAllProducts(ProductParameter productParameter, bool trackChanges)
    {
        var products = await FindByCondition(c => c.IsActive, trackChanges)
            .Filter(productParameter.CategoryGuid)
            .Search(productParameter.SearchTerm)
            .Sort(productParameter.OrderBy!)
            .Skip((productParameter.PageNumber - 1) * productParameter.PageSize)
            .Take(productParameter.PageSize)
            .Include(c => c.Category)
            .Include(c => c.ProductPrices!.Where(x => x.IsActive))
            .ToListAsync();

        var count = await FindByCondition(c => c.IsActive, trackChanges)
            .Filter(productParameter.CategoryGuid)
            .Search(productParameter.SearchTerm)
            .Sort(productParameter.OrderBy!).CountAsync();

        return PagedList<Product>.ToPagedList(products, count, productParameter.PageNumber,
            productParameter.PageSize);
    }

    public async Task<IEnumerable<Product>> SearchAllProducts(string? searchTerm, bool trackChanges)
    {
        return await FindByCondition(c => c.IsActive, trackChanges)
            .Search(searchTerm)
            .Include(c => c.Category)
            .Include(c => c.ProductPrices!.Where(x => x.IsActive))
            .Take(10)
            .ToListAsync();
    }

    public async Task<(string? productName, int stock)> GetProductNameAndStockByGuid(Guid guid, bool trackChanges)
    {
        return await FindByCondition(c => c.Guid.Equals(guid), trackChanges)
            .Select(o => ValueTuple.Create(o.Name, o.Stock))
            .FirstOrDefaultAsync();
    }

    public async Task<string?> GetCurrentProductCode(string code, bool trackChanges)
    {
        return await FindByCondition(c => c.Code!.Contains(code), trackChanges)
            .OrderByDescending(c => c.Code)
            .Select(c => c.Code!)
            .FirstOrDefaultAsync();
    }

    public async Task<int> GetProductStockByProductGuid(Guid guid, bool trackChanges)
    {
        return await FindByCondition(c => c.Guid == guid, trackChanges)
            .Select(c => c.Stock)
            .FirstOrDefaultAsync();
    }
}