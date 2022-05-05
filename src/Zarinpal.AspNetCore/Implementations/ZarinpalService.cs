using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Options;
using Zarinpal.AspNetCore.DTOs;
using Zarinpal.AspNetCore.DTOs.Common;
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
                _zarinpalOptions.MerchantId?.Length == 36 &&
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
                        var jsonObjectResponse =
                            JsonSerializer.Deserialize<JsonObject>(await response.Content.ReadAsStringAsync());
                        if (jsonObjectResponse != null)
                        {
                            var data = jsonObjectResponse["data"] as JsonObject;
                            if (data?.Count > 0)
                            {
                                var requestResult = data.Deserialize<RequestResultData>();
                                if (requestResult?.Code != null)
                                    return new ZarinpalRequestResultDTO(requestResult.Code == 100,
                                        $"https://zarinpal.com/pg/StartPay/{requestResult.Authority}",
                                        (ZarinpalStatusResult)requestResult.Code.Value);
                            }
                            else
                            {
                                // we have error here
                                var errors = jsonObjectResponse["errors"] as JsonObject;
                                if (errors?.Count > 0)
                                {
                                    int? code = errors["code"]?.GetValue<int>();
                                    if (code != null)
                                        return new ZarinpalRequestResultDTO(false,
                                            string.Empty,
                                            (ZarinpalStatusResult)code.Value);
                                }
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

                    var requestResponse = JsonSerializer.Deserialize<SandboxRequestResult>
                        (await response.Content.ReadAsStringAsync());

                    if (requestResponse != null)
                        return new ZarinpalRequestResultDTO(requestResponse.Status == 100,
                            $"https://sandbox.zarinpal.com/pg/StartPay/{requestResponse.Authority}",
                            (ZarinpalStatusResult)requestResponse.Status);
                }
            }

            return new ZarinpalRequestResultDTO(false, string.Empty, ZarinpalStatusResult.St400);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ZarinpalRequestResultDTO(false, string.Empty, ZarinpalStatusResult.St400);
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