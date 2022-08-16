namespace Zarinpal.AspNetCore.Interfaces;

public interface IAdvancedZarinpalService : IDisposable
{
    Task<ZarinpalUnVerifyDTO> UnVerifiedAsync();
}