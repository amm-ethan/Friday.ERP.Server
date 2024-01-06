using Friday.ERP.Core.Data.Entities.InventoryManagement;
using Friday.ERP.Core.IRepositories.Entities.InventoryManagement;
using Microsoft.EntityFrameworkCore;

namespace Friday.ERP.Infrastructure.Repositories.Entities.InventoryManagement;

internal sealed class ProductPriceRepository(RepositoryContext repositoryContext)
    : RepositoryBase<ProductPrice>(repositoryContext),
        IProductPriceRepository
{
    public void CreateProductPrice(ProductPrice productPrice)
    {
        Create(productPrice);
    }

    public async Task<IEnumerable<ProductPrice>> GetAllProductPricesByProductGuid(Guid productGuid,
        bool trackChanges)
    {
        return await FindByCondition(c => c.ProductGuid == productGuid && c.IsActive, trackChanges)
            .ToListAsync();
    }

    public async Task<ProductPrice?> GetProductPriceByGuid(Guid guid, bool trackChanges)
    {
        return await FindByCondition(c => c.Guid == guid, trackChanges)
            .SingleOrDefaultAsync();
    }
}