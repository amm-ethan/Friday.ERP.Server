namespace Friday.ERP.Core.Exceptions.BadRequest;

public sealed class StockNotEnoughBadRequestException()
    : BadRequestException("Db Error : Product not enough in stock.");