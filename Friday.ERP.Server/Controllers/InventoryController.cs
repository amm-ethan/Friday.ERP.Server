using Friday.ERP.Core.IServices;
using Friday.ERP.Server.ActionFilters;
using Friday.ERP.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Friday.ERP.Server.Controllers;

[ApiController]
[Route("api/inventory-management")]
[Authorize]
public class InventoryController(IServiceManager service, IWebHostEnvironment env) : ControllerBase
{
    private readonly string _wwwroot = Path.Combine(env.WebRootPath, "product_images");

    #region Category

    [HttpPost("categories", Name = "CreateCategory")]
    public async Task<IActionResult> CreateCategory(CategoryCreateDto categoryCreateDto)
    {
        var categoryToReturn = await service.InventoryService.CreateCategory(categoryCreateDto);

        return Ok(categoryToReturn);
    }

    [HttpGet("categories", Name = "GetAllCategories")]
    public async Task<IActionResult> GetAllCategories([FromQuery] CategoryParameter categoryParameter)
    {
        var (categoriesToReturn, metaData) = await service.InventoryService.GetAllCategories(categoryParameter);
        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(metaData);
        return Ok(categoriesToReturn);
    }

    [HttpGet("categories/search", Name = "SearchCategories")]
    public async Task<IActionResult> SearchCategories([FromQuery] string? searchTerm)
    {
        var categoriesToReturn = await service.InventoryService.SearchCategories(searchTerm);
        return Ok(categoriesToReturn);
    }

    [HttpGet("categories/{guid:guid}", Name = "GetCategoryByGuid")]
    public async Task<IActionResult> GetCategoryByGuid(Guid guid)
    {
        var categoryToReturn = await service.InventoryService.GetCategoryByGuid(guid);
        return Ok(categoryToReturn);
    }

    [HttpPost("categories/{guid:guid}/disable", Name = "DisableCategoryByGuid")]
    public async Task<IActionResult> DisableCategoryByGuid(Guid guid)
    {
        await service.InventoryService.DisableCategory(guid);
        return Ok();
    }

    [HttpPut("categories/{guid:guid}", Name = "UpdateCategory")]
    [ServiceFilter(typeof(GetCurrentUserGuidActionFilter))]
    public async Task<IActionResult> UpdateCategory(Guid guid, CategoryUpdateDto categoryUpdateDto)
    {
        var userGuid = HttpContext.Items["current_user_id"] as string;
        var categoryToReturn = await service.InventoryService.UpdateCategory(guid, categoryUpdateDto, userGuid!);
        return Ok(categoryToReturn);
    }

    #endregion

    #region Product

    [HttpPost("products", Name = "CreateProduct")]
    public async Task<IActionResult> CreateProduct(ProductCreateDto productCreateDto)
    {
        var productToReturn = await service.InventoryService.CreateProduct(productCreateDto, _wwwroot);

        return Ok(productToReturn);
    }

    [HttpGet("products", Name = "GetAllProducts")]
    public async Task<IActionResult> GetAllProducts([FromQuery] ProductParameter productParameter)
    {
        var (productsToReturn, metaData) = await service.InventoryService.GetAllProducts(productParameter, _wwwroot);
        Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(metaData);
        return Ok(productsToReturn);
    }

    [HttpGet("products/search", Name = "SearchProducts")]
    public async Task<IActionResult> SearchProducts([FromQuery] string? searchTerm)
    {
        var productsToReturn = await service.InventoryService.SearchProducts(searchTerm, _wwwroot);
        return Ok(productsToReturn);
    }

    [HttpGet("products/{guid:guid}", Name = "GetProductByGuid")]
    public async Task<IActionResult> GetProductByGuid(Guid guid)
    {
        var productToReturn = await service.InventoryService.GetProductByGuid(guid, _wwwroot);
        return Ok(productToReturn);
    }

    [HttpPut("products/{guid:guid}", Name = "UpdateProduct")]
    [ServiceFilter(typeof(GetCurrentUserGuidActionFilter))]
    public async Task<IActionResult> UpdateProduct(Guid guid, ProductUpdateDto productUpdateDto)
    {
        var userGuid = HttpContext.Items["current_user_id"] as string;
        var productToReturn = await service.InventoryService.UpdateProduct(guid, productUpdateDto, _wwwroot, userGuid!);
        return Ok(productToReturn);
    }

    [HttpPost("products/{guid:guid}/disable", Name = "DisableProductByGuid")]
    public async Task<IActionResult> DisableProductByGuid(Guid guid)
    {
        await service.InventoryService.DisableProduct(guid);
        return Ok();
    }

    [HttpPut("products/{guid:guid}/prices", Name = "UpdateProductPrice")]
    [ServiceFilter(typeof(GetCurrentUserGuidActionFilter))]
    public async Task<IActionResult> UpdateProductPrice(Guid guid,
        ProductPriceCreateDto productPriceCreateDto)
    {
        var userGuid = HttpContext.Items["current_user_id"] as string;
        var productToReturn =
            await service.InventoryService.UpdateProductPrice(guid, productPriceCreateDto, userGuid!);
        return Ok(productToReturn);
    }

    [HttpGet("products/{guid:guid}/prices", Name = "GetProductPriceHistory")]
    public async Task<IActionResult> GetProductPriceHistory(Guid guid)
    {
        var dataToReturn =
            await service.InventoryService.GetProductPriceHistory(guid);
        return Ok(dataToReturn);
    }

    [HttpPost("products/{guid:guid}/prices/{priceGuid:guid}/disable", Name = "DisableProductPrice")]
    public async Task<IActionResult> DisableProductPrice(Guid _, Guid priceGuid)
    {
        await service.InventoryService.DisableProductPrice(priceGuid);
        return Ok();
    }

    [HttpGet("products/{guid:guid}/prices/suggest", Name = "SuggestProductPrice")]
    public async Task<IActionResult> SuggestProductPrice(Guid guid)
    {
        var dataToReturn = await service.InventoryService.SuggestProductPrice(guid);
        return Ok(dataToReturn);
    }

    #endregion
}