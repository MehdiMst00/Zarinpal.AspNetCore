namespace Zarinpal.AspNetCore.DTOs;

public class ZarinpalRequestResultDTO
{
    public bool IsSuccessStatusCode { get; set; }

    public string RedirectUrl { get; set; }

    public ZarinpalStatusCode? StatusCode { get; set; }

    public ZarinpalRequestData? Data { get; set; }

    public ZarinpalRequestResultDTO(bool isSuccessStatusCode, string redirectUrl,
        ZarinpalStatusCode? statusCode)
    {
        IsSuccessStatusCode = isSuccessStatusCode;
        RedirectUrl = redirectUrl;
        StatusCode = statusCode;
    }
}

public class ZarinpalRequestData
{
    public int Code { get; set; }
    public string Message { get; set; } = null!;
    public string Authority { get; set; } = null!;
    public string FeeType { get; set; } = null!;
    public int Fee { get; set; }
}