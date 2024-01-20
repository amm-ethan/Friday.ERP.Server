namespace Friday.ERP.Shared.DataTransferObjects;

public record NotificationCreateDto(
    string Heading,
    string Body
);

public class NotificationViewDto
{
    public Guid Guid { get; set; }
    public string Heading { get; set; }
    public string Body { get; set; }
    public bool HaveRead { get; set; }
    public DateTime SentAt { get; set; }
}

public record SettingViewDto(
    string? ShopImage,
    string ShopName,
    string? ShopDescription,
    string? AddressOne,
    string? AddressTwo,
    string? PhoneOne,
    string? PhoneTwo,
    string? PhoneThree,
    string? PhoneFour,
    int DefaultProfitPercent,
    int DefaultProfitPercentForWholeSale,
    int MinimumStockMargin,
    bool? SuggestSalePrice);

public record SettingUpdateDto(
    int? DefaultProfitPercent,
    int? DefaultProfitPercentForWholeSale,
    int? MinimumStockMargin,
    bool? SuggestSalePrice
);