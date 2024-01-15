using Friday.ERP.Core.Data.Entities.InventoryManagement;
using Friday.ERP.Core.Exceptions.BadRequest;
using Friday.ERP.Core.Exceptions.NotFound;
using Friday.ERP.Core.IRepositories;
using Friday.ERP.Core.IServices;
using Friday.ERP.Core.IServices.Entities;
using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.RequestFeatures;

namespace Friday.ERP.Infrastructure.Services.Entities;

internal sealed class InventoryService(IRepositoryManager repository, ILoggerManager logger) : IInventoryService
{
    #region Product

    public async Task<ProductViewDto> CreateProduct(ProductCreateDto productCreateDto, string wwwroot)
    {
        var product = Product.FromProductCreateDto(productCreateDto);

        var existingProduct = await repository.Product.GetProductByName(productCreateDto.Name, false);
        if (existingProduct is not null)
            throw new DuplicateEntryBadRequestException(productCreateDto.Name, "name", "product");

        var currentProductCode = await repository.Product.GetCurrentProductCode(productCreateDto.Code, false);

        var productCodeString = productCreateDto.Code;
        var productCodeDigit = 0;
        if (currentProductCode is not null)
        {
            var splitCurrentProductCode = currentProductCode.Split('-');
            productCodeString = splitCurrentProductCode[0];
            productCodeDigit = int.Parse(splitCurrentProductCode[1].TrimStart('0'));
        }

        productCodeDigit++;
        var nextProductCodeDigit = productCodeDigit.ToString().PadLeft(7, '0');
        product.Code = $"{productCodeString}-{nextProductCodeDigit}";

        await ValidateCategory(productCreateDto.CategoryGuid);
        product.CategoryGuid = productCreateDto.CategoryGuid;

        if (productCreateDto.Image is not null)
        {
            var imageName = $"{product.Code}_{Guid.NewGuid():N}.png";
            var imagePath = Path.Combine(wwwroot, imageName);
            var byteArray = Convert.FromBase64String(productCreateDto.Image!);
            await File.WriteAllBytesAsync(imagePath, byteArray);
            product.Image = imageName;
        }

        repository.Product.CreateProduct(product);

        // if (productCreateDto.Price is not null)
        // {
        //     var productPrice =
        //         ProductPrice.FromProductPriceCreateDto(productCreateDto.Price);
        //     productPrice.ProductGuid = product.Guid;
        //     repository.ProductPrice.CreateProductPrice(productPrice);
        // }

        await repository.SaveAsync();

        return await GetProductByGuid(product.Guid, wwwroot);
    }

    public async Task<(List<ProductViewDto>, MetaData metaData)> GetAllProducts(ProductParameter productParameter,
        string wwwroot)
    {
        var createdProducts = await repository.Product.GetAllProducts(productParameter, false);
        var dataToReturn = new List<ProductViewDto>();
        foreach (var createdProduct in createdProducts)
            if (createdProduct.Image is not null)
            {
                var imagePath = Path.Combine(wwwroot, createdProduct.Image!);
                if (File.Exists(imagePath))
                {
                    var imageBytes = await File.ReadAllBytesAsync(imagePath);
                    var base64Image = Convert.ToBase64String(imageBytes);
                    dataToReturn.Add(ToProductViewDto(createdProduct, base64Image));
                }
                else
                {
                    dataToReturn.Add(ToProductViewDto(createdProduct, null));
                }
            }
            else
            {
                dataToReturn.Add(ToProductViewDto(createdProduct, null));
            }

        return (dataToReturn, metaData: createdProducts.MetaData);
    }

    public async Task<List<ProductViewDto>> SearchProducts(string? searchTerm, string wwwroot)
    {
        var createdProducts =
            await repository.Product.SearchAllProducts(searchTerm, false);
        var dataToReturn = new List<ProductViewDto>();
        foreach (var createdProduct in createdProducts)
            if (createdProduct.Image is not null)
            {
                var imagePath = Path.Combine(wwwroot, createdProduct.Image!);
                if (File.Exists(imagePath))
                {
                    var imageBytes = await File.ReadAllBytesAsync(imagePath);
                    var base64Image = Convert.ToBase64String(imageBytes);
                    dataToReturn.Add(ToProductViewDto(createdProduct, base64Image));
                }
                else
                {
                    dataToReturn.Add(ToProductViewDto(createdProduct, null));
                }
            }
            else
            {
                dataToReturn.Add(ToProductViewDto(createdProduct, null));
            }

        return dataToReturn;
    }

    public async Task<ProductViewDto> GetProductByGuid(Guid productGuid, string wwwroot)
    {
        var product = await repository.Product.GetProductByGuid(productGuid, false);

        if (product is null)
            throw new ObjectNotFoundByFilterException("productGuid", "Product",
                productGuid.ToString());
        if (product.Image is null) return ToProductViewDto(product, null);
        var imagePath = Path.Combine(wwwroot, product.Image!);
        if (!File.Exists(imagePath)) return ToProductViewDto(product, null);
        var imageBytes = await File.ReadAllBytesAsync(imagePath);
        var base64Image = Convert.ToBase64String(imageBytes);
        return ToProductViewDto(product, base64Image);
    }

    public async Task<(string? productName, int stock)> GetProductNameAndStockByGuid(Guid productGuid)
    {
        return await repository.Product.GetProductNameAndStockByGuid(productGuid, false);
    }

    public async Task<ProductViewDto> UpdateProduct(Guid productGuid, ProductUpdateDto productUpdateDto,
        string wwwroot, string currentUserGuid)
    {
        var product = await ValidateProduct(productGuid, true);

        if (productUpdateDto.Name is not null)
            product.Name = productUpdateDto.Name;

        if (productUpdateDto.TotalStockLeft is not null)
            product.Stock = productUpdateDto.TotalStockLeft ?? 0;

        if (productUpdateDto.Image is not null)
        {
            var imageName = $"{product.Code}_{Guid.NewGuid():N}.png";
            var imagePath = Path.Combine(wwwroot, imageName);
            var byteArray = Convert.FromBase64String(productUpdateDto.Image!);
            await File.WriteAllBytesAsync(imagePath, byteArray);
            product.Image = imageName;
        }

        if (productUpdateDto.CategoryGuid is not null)
        {
            await ValidateCategory(productUpdateDto.CategoryGuid ?? Guid.Empty);
            product.CategoryGuid = productUpdateDto.CategoryGuid;
        }

        await repository.SaveAsync();

        logger.LogInfo($"Product {product.Guid} is Updated by UserId {currentUserGuid}");
        return await GetProductByGuid(productGuid, wwwroot);
    }

    public async Task<ProductPriceViewDto> UpdateProductPrice(Guid productGuid,
        ProductPriceCreateDto productPriceCreateDto, string currentUserGuid)
    {
        var product = await ValidateProduct(productGuid);

        var productPrice = ProductPrice.FromProductPriceCreateDto(productPriceCreateDto);
        productPrice.ProductGuid = productGuid;
        repository.ProductPrice.CreateProductPrice(productPrice);
        await repository.SaveAsync();

        logger.LogInfo($"Price of Product {product.Name} is Updated by UserId {currentUserGuid}");

        return new ProductPriceViewDto(
            productPrice.Guid,
            productPriceCreateDto.IsWholeSale,
            productPriceCreateDto.SalePrice
        );
    }

    public async Task<List<ProductPriceViewDto>> GetProductPriceHistory(Guid productGuid)
    {
        var priceHistory =
            await repository.ProductPrice.GetAllProductPricesByProductGuid(productGuid,
                false);

        return priceHistory.Select(ToProductPriceViewDto).ToList();
    }

    public async Task DisableProduct(Guid productGuid)
    {
        var product = await ValidateProduct(productGuid, true);
        product.IsActive = false;
        await repository.SaveAsync();
    }

    public async Task DisableProductPrice(Guid productPriceGuid)
    {
        var productPrice = await repository.ProductPrice.GetProductPriceByGuid(productPriceGuid, true);
        if (productPrice is null)
            throw new ObjectNotFoundByFilterException("productPriceGuid", "productPrice",
                productPriceGuid.ToString());
        productPrice.IsActive = false;
        await repository.SaveAsync();
    }

    public async Task<SuggestedProductPriceViewDto> SuggestProductPrice(Guid productGuid)
    {
        var setting = await repository.Setting.GetSetting(false);
        if (!setting!.SuggestSalePrice)
            return new SuggestedProductPriceViewDto
            (
                -1,
                -1
            );

        await ValidateProduct(productGuid);
        var today = DateTime.Today.AddDays(1);
        var requiredDate = today.AddDays(-180);
        var invoices =
            await repository.InvoicePurchaseProduct.GetTotalPurchaseInfoOfProductByProductGuidAndBetweenDate(
                productGuid, requiredDate, today, false);

        var invoicePurchaseProducts = invoices.ToList();
        var totalSumOfPurchasedPrice = invoicePurchaseProducts.Sum(c => c.PurchasedPrice);
        var totalCount = invoicePurchaseProducts.Count;

        if (totalCount == 0)
            return new SuggestedProductPriceViewDto
            (
                -1,
                -1
            );

        var averagePrice = totalSumOfPurchasedPrice / totalCount;
        var system = await repository.Setting.GetSetting(false);
        var defaultProfitPercent = (double)system!.DefaultProfitPercent / 100;
        var defaultProfitPercentForWholeSale = (double)system.DefaultProfitPercentForWholeSale / 100;

        return new SuggestedProductPriceViewDto
        (
            (long)(averagePrice * defaultProfitPercent) + averagePrice,
            (long)(averagePrice * defaultProfitPercentForWholeSale) + averagePrice
        );
    }

    #endregion

    #region Category

    public async Task<CategoryViewDto> CreateCategory(CategoryCreateDto categoryCreateDto)
    {
        var category = Category.FromCategoryCreateDto(categoryCreateDto);

        var existingCategory = await repository.Category.GetCategoryByName(categoryCreateDto.Name, false);
        if (existingCategory is not null)
            throw new DuplicateEntryBadRequestException(categoryCreateDto.Name, "name", "category");

        repository.Category.CreateCategory(category);
        await repository.SaveAsync();

        return await GetCategoryByGuid(category.Guid);
    }

    public async Task<CategoryViewDto> UpdateCategory(Guid categoryGuid, CategoryUpdateDto categoryUpdateDto,
        string currentUserGuid)
    {
        var category = await ValidateCategory(categoryGuid, true);

        if (categoryUpdateDto.Name is not null)
            category.Name = categoryUpdateDto.Name;
        if (categoryUpdateDto.Color is not null)
            category.Color = categoryUpdateDto.Color;

        await repository.SaveAsync();

        logger.LogInfo($"Category {category.Guid} is Updated by UserId {currentUserGuid}");

        return await GetCategoryByGuid(categoryGuid);
    }

    public async Task DisableCategory(Guid categoryGuid)
    {
        var category = await ValidateCategory(categoryGuid, true);
        if (category.Products!.Count != 0)
            throw new GeneralBadRequestException("Has related products. Can't Delete");

        category.IsActive = false;
        await repository.SaveAsync();
    }


    public async Task<CategoryViewDto> GetCategoryByGuid(Guid categoryGuid)
    {
        var category = await ValidateCategory(categoryGuid);
        return ToCategoryViewDto(category);
    }

    public async Task<(List<CategoryViewDto>, MetaData metaData)> GetAllCategories(CategoryParameter categoryParameter)
    {
        var createdCategories = await repository.Category.GetAllCategories(categoryParameter, false);
        return (createdCategories.Select(ToCategoryViewDto).ToList(), metaData: createdCategories.MetaData);
    }

    public async Task<List<CategoryViewDto>> SearchCategories(string? searchTerm)
    {
        var createdCategories = await repository.Category.SearchAllCategories(searchTerm, false);
        return createdCategories.Select(ToCategoryViewDto).ToList();
    }

    #endregion

    #region Private Methods

    private async Task<Category> ValidateCategory(Guid categoryGuid, bool trackChanges = false)
    {
        var category = await repository.Category.GetCategoryByGuid(categoryGuid, trackChanges);
        if (category is null)
            throw new ObjectNotFoundByFilterException("categoryGuid", "Category",
                categoryGuid.ToString());
        return category;
    }

    private static CategoryViewDto ToCategoryViewDto(Category category)
    {
        return new CategoryViewDto
        (
            category.Guid,
            category.Name!,
            category.Color!
        );
    }

    private async Task<Product> ValidateProduct(Guid productGuid, bool trackChanges = false)
    {
        var product = await repository.Product.GetProductByGuid(productGuid, trackChanges);
        if (product is null)
            throw new ObjectNotFoundByFilterException("productGuid", "Product",
                productGuid.ToString());
        return product;
    }

    private static ProductPriceViewDto ToProductPriceViewDto(
        ProductPrice productPrice)
    {
        return new ProductPriceViewDto
        (
            productPrice.Guid,
            productPrice.IsWholeSale,
            productPrice.SalePrice
        );
    }

    private static ProductViewDto ToProductViewDto(Product product, string? image)
    {
        var priceList = product.ProductPrices!
            .Select(ToProductPriceViewDto).ToList();

        return new ProductViewDto
        (
            product.Guid,
            product.Code!,
            product.Name!,
            product.Stock,
            image,
            Prices: priceList,
            CategoryGuid: product.Category!.Guid,
            CategoryName: product.Category!.Name!
        );
    }

    #endregion
}