namespace Zarinpal.AspNetCore.DTOs;

public class ZarinpalRequestDTO
{
    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public int Amount { get; set; }

    public string? Description { get; set; }

    public string? VerifyCallbackUrl { get; set; }

    public string? OrderId { get; set; }

    public string? CardPan { get; set; }

    public List<ZarinpalWages>? Wages { get; set; }

    public ZarinpalRequestDTO() { }

    public ZarinpalRequestDTO(int amount,
        string description, 
        string verifyCallbackUrl,
        string? email = null,
        string? mobile = null,
        string? orderId = null,
        string? cardPan = null,
        List<ZarinpalWages>? wages = null)
    {
        Amount = amount;
        Description = description;
        VerifyCallbackUrl = verifyCallbackUrl;
        Email = email;
        Mobile = mobile;
        OrderId = orderId;
        CardPan = cardPan;
        Wages = wages;
    }
}