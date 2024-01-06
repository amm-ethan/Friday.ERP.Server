using Friday.ERP.Core.Data.Entities.InventoryManagement;
using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.RequestFeatures;

namespace Friday.ERP.Core.IRepositories.Entities.InventoryManagement;

public interface IProductRepository
{
    void CreateProduct(Product product);

    Task<Product?> GetProductByName(string name, bool trackChanges);

    Task<Product?> GetProductByGuid(Guid guid, bool trackChanges);

    Task<PagedList<Product>> GetAllProducts(ProductParameter productParameter, bool trackChanges);

    Task<IEnumerable<Product>> SearchAllProducts(string? searchTerm, bool trackChanges);

    Task<(string? productName, int stock)> GetProductNameAndStockByGuid(Guid guid, bool trackChanges);

    Task<string?> GetCurrentProductCode(string code, bool trackChanges);

    Task<int> GetProductStockByProductGuid(Guid guid, bool trackChanges);
}