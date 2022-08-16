namespace Zarinpal.AspNetCore.Interfaces;

public interface IZarinpalService : IDisposable
{
    Task<ZarinpalRequestResultDTO> RequestAsync(ZarinpalRequestDTO request);
    Task<ZarinpalVerifyResultDTO> VerifyAsync(ZarinpalVerifyDTO verify);
}