namespace Zarinpal.AspNetCore.DTOs;

public class ZarinpalRequestDTO
{
    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public int Amount { get; set; }

    public string? Description { get; set; }

    public string? VerifyCallbackUrl { get; set; }

    public string? OrderId { get; set; }

    public ZarinpalRequestDTO() { }

    public ZarinpalRequestDTO(int amount, string description, string verifyCallbackUrl,
        string? email = null, string? mobile = null, string? orderId = null)
    {
        Amount = amount;
        Description = description;
        VerifyCallbackUrl = verifyCallbackUrl;
        Email = email;
        Mobile = mobile;
        OrderId = orderId;
    }
}