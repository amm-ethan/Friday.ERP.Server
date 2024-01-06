using Friday.ERP.Shared.RequestFeatures;

namespace Friday.ERP.Shared.DataTransferObjects;

public record CategoryViewDto(
    Guid Guid,
    string Name,
    string Color
);

public record CategoryCreateDto(
    string Name,
    string Color
);

public record CategoryUpdateDto(
    string? Name,
    string? Color
);

public class CategoryParameter : RequestParameters
{
    public CategoryParameter()
    {
        OrderBy = "Name";
    }

    public string? SearchTerm { get; set; }
}