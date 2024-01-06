using Friday.ERP.Shared.RequestFeatures;

namespace Friday.ERP.Shared.DataTransferObjects;


public record CustomerVendorCreateDto(
    string Name,
    string? Phone,
    string? Email,
    string? Address,
    long TotalCreditDebitLeft
);

public record CustomerVendorViewDto(
    Guid Guid,
    string Code,
    string Name,
    string? Phone,
    string? Email,
    string? Address,
    long TotalCreditDebitLeft
    );

public record CustomerVendorUpdateDto(
    string? Phone,
    string? Email,
    string? Address,
    long? TotalCreditDebitLeft
);

public class CustomerVendorParameter : RequestParameters
{
    public CustomerVendorParameter()
    {
        OrderBy = "Name";
    }

    public string? SearchTerm { get; set; }
}