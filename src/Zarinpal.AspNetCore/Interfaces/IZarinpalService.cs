using Zarinpal.AspNetCore.DTOs;

namespace Zarinpal.AspNetCore.Interfaces;

public interface IZarinpalService : IDisposable
{
    Task<ZarinpalRequestResultDTO> RequestAsync(ZarinpalRequestDTO request);
}