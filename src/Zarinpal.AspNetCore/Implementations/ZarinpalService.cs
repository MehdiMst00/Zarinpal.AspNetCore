using System.Dynamic;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Zarinpal.AspNetCore.DTOs;
using Zarinpal.AspNetCore.Interfaces;
using Zarinpal.AspNetCore.Models;

namespace Zarinpal.AspNetCore.Implementations;

public class ZarinpalService : IZarinpalService
{
    #region constrcutor

    private readonly HttpClient _httpClient;
    private readonly ZarinpalOptions _zarinpalOptions;

    public ZarinpalService(HttpClient httpClient,
        IOptions<ZarinpalOptions> options)
    {
        _httpClient = httpClient;
        _zarinpalOptions = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    #endregion

    #region zarinpal

    public async Task<ZarinpalRequestResultDTO> RequestAsync(ZarinpalRequestDTO request)
    {
        try
        {
            if (request.Amount > 0 &&
                !string.IsNullOrEmpty(request.Description) &&
                !string.IsNullOrEmpty(request.VerifyCallbackUrl) &&
                (request.VerifyCallbackUrl.ToLower().StartsWith("http://") ||
                request.VerifyCallbackUrl.ToLower().StartsWith("https://")))
            {
                var requestDto = new RequestDTO
                {
                    MerchantId = _zarinpalOptions.MerchantId,
                    Amount = request.Amount,
                    Description = request.Description,
                    VerifyCallbackUrl = request.VerifyCallbackUrl,
                    Email = !string.IsNullOrEmpty(request.Email) ? request.Email : "",
                    Mobile = !string.IsNullOrEmpty(request.Mobile) ? request.Mobile : "",
                };

                var response = await _httpClient.PostAsJsonAsync("v4/payment/request.json", requestDto);
                if (response.IsSuccessStatusCode)
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    dynamic? expandoObject = JsonSerializer.Deserialize<ExpandoObject>(stringResponse) ?? null;

                    if (expandoObject?.errors.ToString() == "[]")
                    {
                        var requestResponse = JsonSerializer.Deserialize<RequestResult>(stringResponse);
                        if (requestResponse?.Data?.Code == 100)
                        {
                            if (_zarinpalOptions.ZarinpalMode == ZarinpalMode.Original)
                                return new ZarinpalRequestResultDTO(true,
                                    $"https://zarinpal.com/pg/StartPay/{requestResponse.Data.Authority}");
                            else
                                return new ZarinpalRequestResultDTO(true,
                                    $"https://sandbox.zarinpal.com/pg/StartPay/{requestResponse.Data.Authority}");
                        }
                    }
                }
            }

            return new ZarinpalRequestResultDTO(false, string.Empty);
        }
        catch
        {
            return new ZarinpalRequestResultDTO(false, string.Empty);
        }
    }

    #endregion

    #region dispose

    public void Dispose()
    {
        _httpClient?.Dispose();
    }

    #endregion
}