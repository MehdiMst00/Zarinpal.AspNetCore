namespace Zarinpal.AspNetCore.Models;

public class ZarinpalWages
{
    [JsonPropertyName("iban")]
    public string? Iban { get; set; }

    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    public ZarinpalWages() { }

    public ZarinpalWages(string iban, 
        int amount,
        string description)
    {
        Iban = iban;
        Amount = amount;
        Description = description;
    }
}
