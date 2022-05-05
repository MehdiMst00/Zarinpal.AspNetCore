using System.Dynamic;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Zarinpal.AspNetCore.DTOs;
using Zarinpal.AspNetCore.DTOs.Sandbox;
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
                if (_zarinpalOptions.ZarinpalMode == ZarinpalMode.Original)
                {
                    // Original Request
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
                                return new ZarinpalRequestResultDTO(true,
                                    $"https://zarinpal.com/pg/StartPay/{requestResponse.Data.Authority}");
                            }
                        }
                    }
                }
                else
                {
                    // Sandbox Request
                    var response = await _httpClient.PostAsJsonAsync("rest/WebGate/PaymentRequest.json", new SandboxRequest
                    {
                        MerchantID = _zarinpalOptions.MerchantId,
                        Amount = request.Amount,
                        CallbackURL = request.VerifyCallbackUrl,
                        Description = request.Description,
                    });

                    if (response.IsSuccessStatusCode)
                    {
                        var requestResponse = JsonSerializer.Deserialize<SandboxRequestResult>
                            (await response.Content.ReadAsStringAsync());

                        if (requestResponse?.Status == 100)
                            return new ZarinpalRequestResultDTO(true,
                                $"https://sandbox.zarinpal.com/pg/StartPay/{requestResponse.Authority}");
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

    public async Task<ZarinpalVerifyResultDTO> VerifyAsync(ZarinpalVerifyDTO verify)
    {
        try
        {
            if (!string.IsNullOrEmpty(verify.Authority) && verify.Amount > 0)
            {
                VerifyDTO verifyDto = new()
                {
                    Amount = verify.Amount,
                    Authority = verify.Authority,
                    MerchantId = _zarinpalOptions.MerchantId
                };

                var response = await _httpClient.PostAsJsonAsync("v4/payment/verify.json", verifyDto);

                if (response.IsSuccessStatusCode)
                {
                    var verificationResponse =
                        await JsonSerializer.DeserializeAsync<VerifyResult>
                            (await response.Content.ReadAsStreamAsync());
                    if (verificationResponse?.Data?.Code is 100)
                        return new ZarinpalVerifyResultDTO(true, verificationResponse.Data.RefId);
                }
            }

            return new ZarinpalVerifyResultDTO(false);
        }
        catch
        {
            return new ZarinpalVerifyResultDTO(false);
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