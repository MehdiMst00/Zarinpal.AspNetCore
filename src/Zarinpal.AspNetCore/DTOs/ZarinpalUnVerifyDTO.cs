namespace Zarinpal.AspNetCore.DTOs;

public class ZarinpalUnVerifyDTO
{
    public ZarinpalUnVerifyDTO() { }

    public ZarinpalUnVerifyDTO(bool isSuccessStatusCode)
    {
        IsSuccessStatusCode = isSuccessStatusCode;
    }

    public ZarinpalUnVerifyDTO(bool isSuccessStatusCode, ZarinpalStatusCode? statusCode)
    {
        IsSuccessStatusCode = isSuccessStatusCode;
        StatusCode = statusCode;
    }

    public ZarinpalUnVerifyDTO(bool isSuccessStatusCode, ZarinpalStatusCode? statusCode,
        List<ZarinpalUnVerifyAuthority>? authorities)
    {
        IsSuccessStatusCode = isSuccessStatusCode;
        StatusCode = statusCode;
        Authorities = authorities;
    }

    public bool IsSuccessStatusCode { get; set; }

    public ZarinpalStatusCode? StatusCode { get; set; }

    public List<ZarinpalUnVerifyAuthority>? Authorities { get; set; }
}

public class ZarinpalUnVerifyAuthority
{
    [JsonPropertyName("authority")]
    public string? Authority { get; set; }

    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("callback_url")]
    public string? CallbackUrl { get; set; }

    [JsonPropertyName("referer")]
    public string? Referer { get; set; }

    [JsonPropertyName("date")]
    public string? Date { get; set; }

    [JsonIgnore]
    public DateTime DateTime => DateTime.Parse(Date!);
}