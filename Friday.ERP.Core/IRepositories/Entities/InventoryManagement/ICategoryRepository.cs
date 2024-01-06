using Friday.ERP.Core.Data.Entities.InventoryManagement;
using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.RequestFeatures;

namespace Friday.ERP.Core.IRepositories.Entities.InventoryManagement;

public interface ICategoryRepository
{
    void CreateCategory(Category category);

    Task<Category?> GetCategoryByGuid(Guid guid, bool trackChanges);

    Task<Category?> GetCategoryByName(string name, bool trackChanges);

    Task<PagedList<Category>> GetAllCategories(CategoryParameter categoryParameter, bool trackChanges);

    Task<IEnumerable<Category>> SearchAllCategories(string? searchTerm, bool trackChanges);
}