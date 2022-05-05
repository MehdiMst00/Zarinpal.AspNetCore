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
                    var response = await _httpClient.PostAsJsonAsync("v4/payment/request.json",
                        new RequestDTO
                        {
                            MerchantId = _zarinpalOptions.MerchantId,
                            Amount = request.Amount,
                            Description = request.Description,
                            VerifyCallbackUrl = request.VerifyCallbackUrl,
                            Email = !string.IsNullOrEmpty(request.Email) ? request.Email : "",
                            Mobile = !string.IsNullOrEmpty(request.Mobile) ? request.Mobile : "",
                        });

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
                                        (ZarinpalStatusCode)requestResult.Code);
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
                                            (ZarinpalStatusCode)code);
                                }
                            }
                        }
                    }
                }
                else
                {
                    // Sandbox Request
                    var response = await _httpClient.PostAsJsonAsync("rest/WebGate/PaymentRequest.json", new SandboxRequestDTO
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
                            (ZarinpalStatusCode)requestResponse.Status);
                }
            }

            return new ZarinpalRequestResultDTO(false, string.Empty, ZarinpalStatusCode.St400);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new ZarinpalRequestResultDTO(false, string.Empty, ZarinpalStatusCode.St400);
        }
    }

    public async Task<ZarinpalVerifyResultDTO> VerifyAsync(ZarinpalVerifyDTO verify)
    {
        try
        {
            if (!string.IsNullOrEmpty(verify.Authority) && verify.Amount > 0)
            {
                if (_zarinpalOptions.ZarinpalMode == ZarinpalMode.Original)
                {
                    // Original Verify
                    var response = await _httpClient.PostAsJsonAsync("v4/payment/verify.json",
                        new VerifyDTO
                        {
                            Amount = verify.Amount,
                            Authority = verify.Authority,
                            MerchantId = _zarinpalOptions.MerchantId
                        });

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonObjectResponse =
                            JsonSerializer.Deserialize<JsonObject>(await response.Content.ReadAsStringAsync());
                        if (jsonObjectResponse != null)
                        {
                            var data = jsonObjectResponse["data"] as JsonObject;
                            if (data?.Count > 0)
                            {
                                var requestResult = data.Deserialize<VerifyResultData>();
                                if (requestResult?.Code != null)
                                    return new ZarinpalVerifyResultDTO(requestResult.Code == 100,
                                        requestResult.RefId,
                                        (ZarinpalStatusCode)requestResult.Code);
                            }
                            else
                            {
                                // we have error here
                                var errors = jsonObjectResponse["errors"] as JsonObject;
                                if (errors?.Count > 0)
                                {
                                    int? code = errors["code"]?.GetValue<int>();
                                    if (code != null)
                                        return new ZarinpalVerifyResultDTO(false,
                                            null,
                                            (ZarinpalStatusCode)code);
                                }
                            }
                        }
                    }
                }
                else
                {
                    // Sandbox Verify
                    var response = await _httpClient.PostAsJsonAsync("rest/WebGate/PaymentVerification.json",
                        new SandboxVerifyDTO
                        {
                            Amount = verify.Amount,
                            Authority = verify.Authority,
                            MerchantId = _zarinpalOptions.MerchantId
                        });

                    if (response.IsSuccessStatusCode)
                    {
                        var verifyResponse =
                            await JsonSerializer.DeserializeAsync<SandboxVerifyResult>
                                (await response.Content.ReadAsStreamAsync());

                        if (verifyResponse != null)
                            return new ZarinpalVerifyResultDTO(verifyResponse.Status == 100,
                                verifyResponse.RefID,
                                (ZarinpalStatusCode)verifyResponse.Status);
                    }
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