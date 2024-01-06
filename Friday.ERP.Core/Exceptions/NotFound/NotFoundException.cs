namespace Friday.ERP.Core.Exceptions.NotFound;

public abstract class NotFoundException(string message) : Exception(message);