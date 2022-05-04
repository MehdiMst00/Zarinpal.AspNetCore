namespace Zarinpal.AspNetCore.Models;

public class ZarinpalOptions
{
    public string? MerchantId { get; set; }
    public ZarinpalMode ZarinpalMode { get; set; }
}

public enum ZarinpalMode
{
    Sandbox,
    Original
}