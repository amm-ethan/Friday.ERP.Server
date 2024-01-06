using Newtonsoft.Json;

namespace Friday.ERP.Shared;

public record ErrorResponseDto<T> where T : class
{
    [JsonProperty("error_reference_number")]
    public Guid ErrorReferenceNumber { get; set; }

    [JsonProperty("error_type")] public string? ErrorType { get; set; }

    [JsonProperty("error_details")] public List<T>? ErrorDetail { get; init; }
}