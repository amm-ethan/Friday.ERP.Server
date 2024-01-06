namespace Friday.ERP.Core.Exceptions.NotFound;

public sealed class ObjectNotFoundByFilterException(string objectName, string type, string value)
    : NotFoundException($"{objectName} with {type}: {value} doesn't exist in this application.");