using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.RequestFeatures;

namespace Friday.ERP.Core.IServices.Entities;

public interface IInventoryService
{
    #region Category

    Task<CategoryViewDto> CreateCategory(CategoryCreateDto categoryCreateDto);

    Task<CategoryViewDto> UpdateCategory(Guid categoryGuid, CategoryUpdateDto categoryUpdateDto,
        string currentUserGuid);

    Task<CategoryViewDto> GetCategoryByGuid(Guid categoryGuid);

    Task<(List<CategoryViewDto>, MetaData metaData)> GetAllCategories(CategoryParameter categoryParameter);

    Task<List<CategoryViewDto>> SearchCategories(string? searchTerm);

    Task DisableCategory(Guid categoryGuid);

    #endregion

    #region Product

    Task<ProductViewDto> CreateProduct(ProductCreateDto productCreateDto, string wwwroot);

    Task<(List<ProductViewDto>, MetaData metaData)> GetAllProducts(ProductParameter productParameter, string wwwroot);

    Task<List<ProductViewDto>> SearchProducts(string? searchTerm, string wwwroot);

    Task<ProductViewDto> GetProductByGuid(Guid productGuid, string wwwroot);

    Task<(string? productName, int stock)> GetProductNameAndStockByGuid(Guid productGuid);

    Task<ProductViewDto> UpdateProduct(Guid productGuid, ProductUpdateDto productUpdateDto,
        string wwwroot, string currentUserGuid);

    Task<ProductPriceViewDto> UpdateProductPrice(Guid productGuid,
        ProductPriceCreateDto productPriceCreateDto, string currentUserGuid);

    Task<List<ProductPriceViewDto>> GetProductPriceHistory(Guid productGuid);

    Task DisableProduct(Guid productGuid);

    Task DisableProductPrice(Guid productPriceGuid);

    Task<SuggestedProductPriceViewDto> SuggestProductPrice(Guid productGuid);

    #endregion
}