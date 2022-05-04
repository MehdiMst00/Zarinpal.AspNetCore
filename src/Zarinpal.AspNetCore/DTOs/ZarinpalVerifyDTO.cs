namespace Zarinpal.AspNetCore.DTOs;

public class ZarinpalVerifyDTO
{
    public int Amount { get; set; }

    public string Authority { get; set; }

    public ZarinpalVerifyDTO(int amount, string authority)
    {
        Amount = amount;
        Authority = authority;
    }
}