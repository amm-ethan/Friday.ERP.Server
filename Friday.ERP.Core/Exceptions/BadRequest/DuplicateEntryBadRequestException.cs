namespace Friday.ERP.Core.Exceptions.BadRequest;

public sealed class DuplicateEntryBadRequestException(string value, string column, string table)
    : BadRequestException($"Db Error : Duplicate value {value} in {column} : {table}");