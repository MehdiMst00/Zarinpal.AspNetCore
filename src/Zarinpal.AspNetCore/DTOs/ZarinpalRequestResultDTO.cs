using Zarinpal.AspNetCore.DTOs.Common;

namespace Zarinpal.AspNetCore.DTOs;

public class ZarinpalRequestResultDTO
{
    public bool IsSuccessStatusCode { get; set; }

    public string RedirectUrl { get; set; }

    public ZarinpalStatusResult StatusCode { get; set; }

    public ZarinpalRequestResultDTO(bool isSuccessStatusCode, string redirectUrl,
        ZarinpalStatusResult statusCode)
    {
        IsSuccessStatusCode = isSuccessStatusCode;
        RedirectUrl = redirectUrl;
        StatusCode = statusCode;
    }
}