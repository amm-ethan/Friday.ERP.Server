using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Friday.ERP.Core.Data.Entities.SystemManagement;

[Table("sm_setting")]
public class Setting
{
    [Key] public Guid Guid { get; set; }

    [Column("image")] public string? Image { get; set; }

    [Required]
    [Column("name", TypeName = "varchar(256)")]
    public string? Name { get; set; }

    [Column("description", TypeName = "varchar(256)")]
    public string? Description { get; set; }

    [Column("address_one", TypeName = "varchar(256)")]
    public string? AddressOne { get; set; }

    [Column("address_two", TypeName = "varchar(256)")]
    public string? AddressTwo { get; set; }

    [Column("phone_one", TypeName = "varchar(100)")]
    public string? PhoneOne { get; set; }

    [Column("phone_two", TypeName = "varchar(100)")]
    public string? PhoneTwo { get; set; }

    [Column("phone_three", TypeName = "varchar(100)")]
    public string? PhoneThree { get; set; }

    [Column("phone_four", TypeName = "varchar(100)")]
    public string? PhoneFour { get; set; }

    [Required]
    [Column("suggest_sale_price")]
    public bool SuggestSalePrice { get; set; } = true;

    [Required]
    [Column("default_profit_percent")]
    public int DefaultProfitPercent { get; set; }

    [Required]
    [Column("default_profit_percent_for_whole_sale")]
    public int DefaultProfitPercentForWholeSale { get; set; }

    [Required]
    [Column("minimum_stock_margin")]
    public int MinimumStockMargin { get; set; }
}