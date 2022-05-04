namespace Zarinpal.AspNetCore.DTOs;

public class ZarinpalRequestResultDTO
{
    public bool IsSuccessStatusCode { get; set; }

    public string RedirectUrl { get; set; }

    public ZarinpalRequestResultDTO(bool isSuccessStatusCode, string redirectUrl)
    {
        IsSuccessStatusCode = isSuccessStatusCode;
        RedirectUrl = redirectUrl;
    }
}