namespace Friday.ERP.Core.Exceptions.BadRequest;

public sealed class GeneralBadRequestException(string errorString) : BadRequestException($"{errorString}");