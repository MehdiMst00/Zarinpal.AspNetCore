using Zarinpal.AspNetCore.DTOs.Common;

namespace Zarinpal.AspNetCore.DTOs;

public class ZarinpalRequestResultDTO
{
    public bool IsSuccessStatusCode { get; set; }

    public string RedirectUrl { get; set; }

    public ZarinpalStatusCode? StatusCode { get; set; }

    public ZarinpalRequestResultDTO(bool isSuccessStatusCode, string redirectUrl,
        ZarinpalStatusCode? statusCode)
    {
        IsSuccessStatusCode = isSuccessStatusCode;
        RedirectUrl = redirectUrl;
        StatusCode = statusCode;
    }
}