using Friday.ERP.Core.Data.Entities.InvoiceManagement;
using Friday.ERP.Core.Exceptions.BadRequest;
using Friday.ERP.Core.Exceptions.NotFound;
using Friday.ERP.Core.IRepositories;
using Friday.ERP.Core.IServices;
using Friday.ERP.Core.IServices.Entities;
using Friday.ERP.Shared.DataTransferObjects;
using Friday.ERP.Shared.Enums;
using Friday.ERP.Shared.RequestFeatures;

namespace Friday.ERP.Infrastructure.Services.Entities;

internal sealed class InvoiceService(IRepositoryManager repository, ILoggerManager logger) : IInvoiceService
{
    public async Task<List<MonthlySaleFigureViewDto>> GetMonthlySummary(int lastTotalMonths = 12)
    {
        var dataToReturn = new List<MonthlySaleFigureViewDto>();

        var last12Months = new List<(int Year, int Month)>();
        var currentDate = DateTime.Now;
        for (var i = 0; i < lastTotalMonths; i++)
        {
            last12Months.Add((currentDate.Year, currentDate.Month));
            currentDate = currentDate.AddMonths(-1);
        }

        last12Months.Reverse();
        foreach (var requiredDate in last12Months)
        {
            var totalPurchase = await repository.InvoicePurchase.GetPurchaseTotalOfSelectedMonthAndYear(
                requiredDate.Month, requiredDate.Year);
            var totalSale = await repository.InvoiceSale.GetSaleTotalOfSelectedMonthAndYear(
                requiredDate.Month, requiredDate.Year);

            var totalProfit = totalSale - totalPurchase;
            dataToReturn.Add(new MonthlySaleFigureViewDto(
                Enum.GetName(typeof(MonthsOfYearEnum), requiredDate.Month),
                requiredDate.Year,
                totalSale,
                totalPurchase,
                totalProfit < 0 ? 0 : totalSale - totalPurchase
            ));
        }

        return dataToReturn;
    }


    #region Invoice_Purchase

    public async Task<InvoicePurchaseViewDto> CreateInvoicePurchase(InvoicePurchaseCreateDto invoicePurchaseCreateDto,
        string wwwroot)
    {
        var invoice = InvoicePurchase.FromInvoicePurchaseCreateDto(invoicePurchaseCreateDto);

        var currentInvoiceId = await repository.InvoicePurchase.GetCurrentInvoiceId(false);
        var invoiceIdString = "P";
        var invoiceIdDigit = 0;
        if (currentInvoiceId is not null)
        {
            var splitCurrentInvoiceId = currentInvoiceId.Split('-');
            invoiceIdString = splitCurrentInvoiceId[0];
            invoiceIdDigit = int.Parse(splitCurrentInvoiceId[1].TrimStart('0'));
        }

        invoiceIdDigit++;
        var nextInvoiceIdDigit = invoiceIdDigit.ToString().PadLeft(7, '0');
        invoice.InvoiceNo = $"{invoiceIdString}-{nextInvoiceIdDigit}";

        var vendor =
            await repository.CustomerVendor.GetCustomerVendorByGuid(invoicePurchaseCreateDto.VendorGuid, false);
        if (vendor is null)
            throw new ObjectNotFoundByFilterException("VendorGuid", "CustomerVendor",
                invoicePurchaseCreateDto.VendorGuid.ToString());
        invoice.VendorGuid = invoicePurchaseCreateDto.VendorGuid;

        foreach (var purchaseProductToCreate in invoicePurchaseCreateDto.PurchasedProducts)
        {
            var invoicePurchaseProductToCreate =
                InvoicePurchaseProduct.FromInvoicePurchaseProductCreateDto(purchaseProductToCreate);

            invoicePurchaseProductToCreate.InvoicePurchaseGuid = invoice.Guid;
            repository.InvoicePurchaseProduct.CreateInvoicePurchaseProduct(invoicePurchaseProductToCreate);

            var product =
                await repository.Product.GetProductByGuid(purchaseProductToCreate.ProductGuid, true);
            product!.Stock += purchaseProductToCreate.Quantity;
        }

        repository.InvoicePurchase.CreateInvoicePurchase(invoice);
        await repository.SaveAsync();
        return await GetInvoicePurchaseByGuid(invoice.Guid, wwwroot);
    }

    public async Task<InvoicePurchaseViewDto> UpdateInvoicePurchase(Guid guid,
        InvoicePurchaseUpdateDto invoicePurchaseUpdateDto, string wwwroot, string currentUserGuid)
    {
        var invoice = await ValidateInvoicePurchase(guid, true);

        if (invoicePurchaseUpdateDto.Discount is not null)
            invoice.Discount = invoicePurchaseUpdateDto.Discount ?? 0;
        if (invoicePurchaseUpdateDto.DeliveryFees is not null)
            invoice.DeliveryFees = invoicePurchaseUpdateDto.DeliveryFees ?? 0;
        if (invoicePurchaseUpdateDto.Total is not null)
            invoice.Total = invoicePurchaseUpdateDto.Total ?? 0;
        if (invoicePurchaseUpdateDto.GrandTotal is not null)
            invoice.GrandTotal = invoicePurchaseUpdateDto.GrandTotal ?? 0;
        if (invoicePurchaseUpdateDto.PaidTotal is not null)
            invoice.PaidTotal = invoicePurchaseUpdateDto.PaidTotal ?? 0;
        if (invoicePurchaseUpdateDto.CreditDebitLeft is not null)
            invoice.CreditDebitLeft = invoicePurchaseUpdateDto.CreditDebitLeft ?? 0;
        if (invoicePurchaseUpdateDto.IsPaid is not null)
            invoice.IsPaid = invoicePurchaseUpdateDto.IsPaid ?? false;

        await repository.SaveAsync();
        logger.LogInfo($"Invoice Purchase {invoice.Guid} is Updated by UserId {currentUserGuid}");
        return await GetInvoicePurchaseByGuid(guid, wwwroot);
    }

    public async Task<(List<InvoicePurchaseTableViewDto>, MetaData metaData)> GetAllInvoicePurchases(
        InvoiceParameter invoiceParameter)
    {
        var allInvoices = await repository.InvoicePurchase.GetAllInvoicePurchases(invoiceParameter, false);
        return (allInvoices.Select(ToInvoicePurchaseTableViewDto).ToList(), metaData: allInvoices.MetaData);
    }

    public async Task<InvoicePurchaseViewDto> GetInvoicePurchaseByGuid(Guid guid, string wwwroot)
    {
        var invoice = await ValidateInvoicePurchase(guid);
        return ToInvoicePurchaseViewDto(invoice, wwwroot);
    }

    #endregion

    #region Invoice Sale

    public async Task<InvoiceSaleViewDto> CreateInvoiceSale(InvoiceSaleCreateDto invoiceSaleCreateDto, string wwwroot)
    {
        foreach (var soldProduct in invoiceSaleCreateDto.SoldProducts)
        {
            var currentProductStock =
                await repository.Product.GetProductStockByProductGuid(soldProduct.ProductGuid, false);

            if (soldProduct.Quantity > currentProductStock)
                throw new StockNotEnoughBadRequestException();
        }

        var invoice = InvoiceSale.FromInvoiceSaleCreateDto(invoiceSaleCreateDto);

        var currentInvoiceId = await repository.InvoiceSale.GetCurrentInvoiceId(false);
        var invoiceIdString = "S";
        var invoiceIdDigit = 0;
        if (currentInvoiceId is not null)
        {
            var splitCurrentInvoiceId = currentInvoiceId.Split('-');
            invoiceIdString = splitCurrentInvoiceId[0];
            invoiceIdDigit = int.Parse(splitCurrentInvoiceId[1].TrimStart('0'));
        }

        invoiceIdDigit++;
        var nextInvoiceIdDigit = invoiceIdDigit.ToString().PadLeft(7, '0');
        invoice.InvoiceNo = $"{invoiceIdString}-{nextInvoiceIdDigit}";

        if (invoiceSaleCreateDto.CustomerGuid is not null)
        {
            var customerGuid = invoiceSaleCreateDto.CustomerGuid ?? Guid.Empty;
            var customer = await repository.CustomerVendor.GetCustomerVendorByGuid(
                customerGuid, false);
            if (customer is null)
                throw new ObjectNotFoundByFilterException("VendorGuid", "CustomerVendor",
                    customerGuid.ToString());
            invoice.CustomerGuid = customerGuid;
        }

        foreach (var soldProduct in invoiceSaleCreateDto.SoldProducts)
        {
            var invoiceSaleProductToCreate = InvoiceSaleProduct
                .FromInvoiceSaleProductCreateDto(soldProduct);
            invoiceSaleProductToCreate.InvoiceSaleGuid = invoice.Guid;
            invoiceSaleProductToCreate.ProductPriceGuid = soldProduct.ProducePriceGuid;

            repository.InvoiceSaleProduct.CreateInvoiceSaleProduct(invoiceSaleProductToCreate);

            var product =
                await repository.Product.GetProductByGuid(soldProduct.ProductGuid, true);
            product!.Stock -= soldProduct.Quantity;
        }

        repository.InvoiceSale.CreateInvoiceSale(invoice);

        if (invoiceSaleCreateDto.DeliveryInfo is not null)
        {
            var deliveryInfo = InvoiceSaleDelivery.FromInvoiceSaleDeliveryCreateDto(invoiceSaleCreateDto.DeliveryInfo);
            deliveryInfo.InvoiceSaleGuid = invoice.Guid;
            repository.InvoiceSaleDelivery.CreateInvoiceSaleDelivery(deliveryInfo);
        }

        await repository.SaveAsync();
        return await GetInvoiceSaleByGuid(invoice.Guid, wwwroot);
    }

    public async Task<InvoiceSaleViewDto> UpdateInvoiceSale(Guid guid, InvoiceSaleUpdateDto invoiceSaleUpdateDto,
        string currentUserGuid, string wwwroot)
    {
        var invoice = await ValidateInvoicePurchase(guid, true);

        if (invoiceSaleUpdateDto.Discount is not null)
            invoice.Discount = invoiceSaleUpdateDto.Discount ?? 0;
        if (invoiceSaleUpdateDto.DeliveryFees is not null)
            invoice.DeliveryFees = invoiceSaleUpdateDto.DeliveryFees ?? 0;
        if (invoiceSaleUpdateDto.Total is not null)
            invoice.Total = invoiceSaleUpdateDto.Total ?? 0;
        if (invoiceSaleUpdateDto.GrandTotal is not null)
            invoice.GrandTotal = invoiceSaleUpdateDto.GrandTotal ?? 0;
        if (invoiceSaleUpdateDto.PaidTotal is not null)
            invoice.PaidTotal = invoiceSaleUpdateDto.PaidTotal ?? 0;
        if (invoiceSaleUpdateDto.CreditDebitLeft is not null)
            invoice.CreditDebitLeft = invoiceSaleUpdateDto.CreditDebitLeft ?? 0;

        await repository.SaveAsync();
        logger.LogInfo($"Invoice Sale {invoice.Guid} is Updated by UserId {currentUserGuid}");
        return await GetInvoiceSaleByGuid(guid, wwwroot);
    }

    public async Task<(List<InvoiceSaleTableViewDto>, MetaData metaData)> GetAllInvoiceSales(
        InvoiceParameter invoiceParameter)
    {
        var allInvoices = await repository.InvoiceSale.GetAllInvoiceSales(invoiceParameter, false);
        return (allInvoices.Select(ToInvoiceSaleTableViewDto).ToList(), metaData: allInvoices.MetaData);
    }

    public async Task<InvoiceSaleViewDto> GetInvoiceSaleByGuid(Guid guid, string wwwroot)
    {
        var invoice = await ValidateInvoiceSale(guid);
        return ToInvoiceSaleViewDto(invoice, wwwroot);
    }

    public async Task<InvoiceSaleDeliveryViewDto> UpdateInvoiceSaleDeliveryByGuid(Guid guid,
        InvoiceSaleDeliveryUpdateDto invoiceSaleDeliveryUpdateDto,
        string currentUserGuid)
    {
        var deliveryInfo = await repository.InvoiceSaleDelivery.GetInvoiceSaleDeliveryByGuid(guid, true);
        if (deliveryInfo is null)
            throw new ObjectNotFoundByFilterException("Guid", "InvoiceSaleDelivery",
                guid.ToString());

        if (invoiceSaleDeliveryUpdateDto.DeliveryServiceName is not null)
            deliveryInfo.DeliveryServiceName = invoiceSaleDeliveryUpdateDto.DeliveryServiceName;
        if (invoiceSaleDeliveryUpdateDto.DeliveryContactPerson is not null)
            deliveryInfo.DeliveryContactPerson = invoiceSaleDeliveryUpdateDto.DeliveryContactPerson;
        if (invoiceSaleDeliveryUpdateDto.DeliveryContactPhone is not null)
            deliveryInfo.DeliveryContactPhone = invoiceSaleDeliveryUpdateDto.DeliveryContactPhone;
        if (invoiceSaleDeliveryUpdateDto.Remark is not null)
            deliveryInfo.Remark = invoiceSaleDeliveryUpdateDto.Remark;

        await repository.SaveAsync();
        logger.LogInfo($"Invoice Sale {deliveryInfo.Guid} is Updated by UserId {currentUserGuid}");

        var updatedDeliveryInfo = await repository.InvoiceSaleDelivery.GetInvoiceSaleDeliveryByGuid(guid, false);
        return ToInvoiceSaleDeliveryViewDto(updatedDeliveryInfo!);
    }

    public async Task<List<InvoiceSaleForFrequentCustomerViewDto>> GetFrequentCustomersFromInvoiceSale(
        DateTime fromLastDays)
    {
        return await repository.InvoiceSale.GetFrequentCustomersFromInvoiceSale(fromLastDays);
    }

    public async Task<List<TopSellingProductsViewDto>> GetTopSellingProductsFromInvoiceSale(DateTime fromLastDays)
    {
        var invoiceSaleGuidList = await repository.InvoiceSale.GetInvoiceSaleGuidsFromLastDays(fromLastDays);
        return await repository.InvoiceSaleProduct.GetTopSellingProductsFromInvoiceSaleGuids(invoiceSaleGuidList);
    }

    public async Task<ProductPurchasePriceViewDto> GetLastPurchasedPriceOfProduct(Guid guid)
    {
        var lastPrice = await repository.InvoicePurchaseProduct.GetLastPurchasedPriceOfProduct(guid);
        return new ProductPurchasePriceViewDto
        (
            guid,
            lastPrice
        );
    }

    public async Task<InvoiceIdViewDto> GetNextPurchaseInvoiceNumber(bool isSale)
    {
        string? currentInvoiceId;
        string? invoiceIdString;
        if (isSale)
        {
            currentInvoiceId = await repository.InvoiceSale.GetCurrentInvoiceId(false);
            invoiceIdString = "P";
        }
        else
        {
            currentInvoiceId = await repository.InvoicePurchase.GetCurrentInvoiceId(false);
            invoiceIdString = "P";
        }

        var invoiceIdDigit = 0;
        if (currentInvoiceId is not null)
        {
            var splitCurrentInvoiceId = currentInvoiceId.Split('-');
            invoiceIdString = splitCurrentInvoiceId[0];
            invoiceIdDigit = int.Parse(splitCurrentInvoiceId[1].TrimStart('0'));
        }

        invoiceIdDigit++;
        var nextInvoiceIdDigit = invoiceIdDigit.ToString().PadLeft(7, '0');
        return new InvoiceIdViewDto($"{invoiceIdString}-{nextInvoiceIdDigit}");
    }

    #endregion

    #region Private Methods

    private static InvoicePurchaseTableViewDto ToInvoicePurchaseTableViewDto(InvoicePurchase invoice)
    {
        return new InvoicePurchaseTableViewDto
        (
            invoice.Guid,
            invoice.InvoiceNo!,
            invoice.GrandTotal,
            invoice.Vendor!.Guid,
            invoice.Vendor!.Name!,
            invoice.PurchasedAt
        );
    }

    private static InvoicePurchaseViewDto ToInvoicePurchaseViewDto(InvoicePurchase invoice, string wwwroot)
    {
        var purchasedProducts = new List<InvoicePurchaseProductViewDto>();
        foreach (var product in invoice.PurchasedProducts!)
        {
            string? image;
            if (product.Product!.Image is null)
            {
                image = null;
            }
            else
            {
                var imagePath = Path.Combine(wwwroot, product.Product!.Image!);
                if (File.Exists(imagePath))
                {
                    var imageBytes = File.ReadAllBytes(imagePath);
                    image = Convert.ToBase64String(imageBytes);
                }
                else
                {
                    image = null;
                }
            }

            purchasedProducts.Add(
                new InvoicePurchaseProductViewDto(
                    product.Guid,
                    product.Product!.Guid,
                    image,
                    product.Product!.Name!,
                    product.Quantity,
                    product.PurchasedPrice,
                    product.Total
                ));
        }

        return new InvoicePurchaseViewDto
        (
            invoice.Guid,
            invoice.InvoiceNo!,
            invoice.SubTotal,
            invoice.Discount,
            invoice.DeliveryFees,
            invoice.Total,
            invoice.GrandTotal,
            invoice.PaidTotal,
            invoice.ExistingCreditDebit,
            invoice.CreditDebitLeft,
            invoice.IsPaid,
            invoice.Remark,
            invoice.PurchasedAt,
            invoice.Vendor!.Guid,
            new CustomerVendorViewDto(
                invoice.Vendor!.Guid,
                invoice.Vendor!.Code!,
                invoice.Vendor!.Name!,
                invoice.Vendor!.Phone,
                invoice.Vendor!.Email,
                invoice.Vendor!.Address,
                invoice.Vendor!.TotalCreditDebitLeft
            ),
            purchasedProducts
        );
    }

    private async Task<InvoicePurchase> ValidateInvoicePurchase(Guid invoicePurchaseGuid, bool trackChanges = false)
    {
        var invoice = await repository.InvoicePurchase.GetInvoicePurchaseByGuid(invoicePurchaseGuid, trackChanges);
        if (invoice is null)
            throw new ObjectNotFoundByFilterException("Guid", "InvoicePurchase",
                invoicePurchaseGuid.ToString());
        return invoice;
    }

    private static InvoiceSaleTableViewDto ToInvoiceSaleTableViewDto(InvoiceSale invoice)
    {
        return new InvoiceSaleTableViewDto
        (
            invoice.Guid,
            invoice.InvoiceNo!,
            invoice.GrandTotal,
            invoice.Customer?.Guid,
            invoice.Customer?.Name!,
            invoice.PurchasedAt
        );
    }

    private static InvoiceSaleDeliveryViewDto ToInvoiceSaleDeliveryViewDto(InvoiceSaleDelivery invoiceSaleDelivery)
    {
        return new InvoiceSaleDeliveryViewDto
        (
            invoiceSaleDelivery.Guid,
            invoiceSaleDelivery.Address!,
            invoiceSaleDelivery.ContactPerson!,
            invoiceSaleDelivery.ContactPhone!,
            invoiceSaleDelivery.DeliveryServiceName!,
            invoiceSaleDelivery.DeliveryContactPerson,
            invoiceSaleDelivery.DeliveryContactPhone,
            invoiceSaleDelivery.Remark
        );
    }

    private static InvoiceSaleViewDto ToInvoiceSaleViewDto(InvoiceSale invoice, string wwwroot)
    {
        var soldProducts = new List<InvoiceSaleProductViewDto>();
        foreach (var product in invoice.SoldProducts!)
        {
            string? image;
            if (product.Product!.Image is null)
            {
                image = null;
            }
            else
            {
                var imagePath = Path.Combine(wwwroot, product.Product!.Image!);
                if (File.Exists(imagePath))
                {
                    var imageBytes = File.ReadAllBytes(imagePath);
                    image = Convert.ToBase64String(imageBytes);
                }
                else
                {
                    image = null;
                }
            }

            soldProducts.Add(
                new InvoiceSaleProductViewDto(
                    product.Guid,
                    product.Product!.Guid,
                    image,
                    product.Product!.Name!,
                    product.Quantity,
                    product.Total,
                    product.ProductPrice!.Guid,
                    product.ProductPrice.SalePrice,
                    product.ProductPrice.IsWholeSale
                ));
        }

        return new InvoiceSaleViewDto
        (
            invoice.Guid,
            invoice.InvoiceNo!,
            invoice.SubTotal,
            invoice.Discount,
            invoice.DeliveryFees,
            invoice.Total,
            invoice.GrandTotal,
            invoice.PaidTotal,
            invoice.ExistingCreditDebit,
            invoice.CreditDebitLeft,
            invoice.Remark,
            invoice.PurchasedAt,
            invoice.Customer?.Guid,
            invoice.Customer is null
                ? null
                : new CustomerVendorViewDto(
                    invoice.Customer!.Guid,
                    invoice.Customer!.Code!,
                    invoice.Customer!.Name!,
                    invoice.Customer!.Phone,
                    invoice.Customer!.Email,
                    invoice.Customer!.Address,
                    invoice.Customer!.TotalCreditDebitLeft
                ),
            soldProducts,
            invoice.InvoiceSaleDelivery is null ? null : ToInvoiceSaleDeliveryViewDto(invoice.InvoiceSaleDelivery!)
        );
    }

    private async Task<InvoiceSale> ValidateInvoiceSale(Guid invoiceSaleGuid, bool trackChanges = false)
    {
        var invoice = await repository.InvoiceSale.GetInvoiceSaleByGuid(invoiceSaleGuid, trackChanges);
        if (invoice is null)
            throw new ObjectNotFoundByFilterException("Guid", "InvoiceSale",
                invoiceSaleGuid.ToString());
        return invoice;
    }

    #endregion
}