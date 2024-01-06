using Friday.ERP.Core.Data.Entities.InventoryManagement;

namespace Friday.ERP.Core.IRepositories.Entities.InventoryManagement;

public interface IProductPriceRepository
{
    void CreateProductPrice(ProductPrice productPrice);

    Task<IEnumerable<ProductPrice>> GetAllProductPricesByProductGuid(Guid productGuid, bool trackChanges);

    Task<ProductPrice?> GetProductPriceByGuid(Guid guid, bool trackChanges);
}