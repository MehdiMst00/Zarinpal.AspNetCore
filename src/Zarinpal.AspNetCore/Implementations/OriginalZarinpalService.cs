namespace Zarinpal.AspNetCore.Implementations;

public class OriginalZarinpalService : IZarinpalService
{
    #region constructor

    private readonly ILogger<OriginalZarinpalService> _logger;
    private readonly HttpClient _httpClient;
    private readonly ZarinpalOptions _zarinpalOptions;

    public OriginalZarinpalService(ILogger<OriginalZarinpalService> logger,
        HttpClient httpClient,
        IOptions<ZarinpalOptions> options)
    {
        _logger = logger;
        _httpClient = httpClient;
        _zarinpalOptions = options.Value;
    }

    #endregion

    #region zarinpal

    public async Task<ZarinpalRequestResultDTO> RequestAsync(ZarinpalRequestDTO request)
    {
        if (!InternalExtension.IsValidZarinpalRequest(request))
        {
            throw new ZarinpalException("لطفا تمام فیلدهای اجباری را به درستی پر کنید !!!");
        }

        try
        {
            var response = await _httpClient.PostAsJsonAsync("v4/payment/request.json",
                new RequestDTO
                {
                    MerchantId = _zarinpalOptions.MerchantId,
                    Amount = request.Amount,
                    Description = request.Description,
                    VerifyCallbackUrl = request.VerifyCallbackUrl,
                    Currency = _zarinpalOptions.Currency,
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
                        {
                            return new ZarinpalRequestResultDTO(requestResult.Code == 100,
                                $"https://zarinpal.com/pg/StartPay/{requestResult.Authority}",
                                (ZarinpalStatusCode)requestResult.Code)
                            {
                                Data = new ZarinpalRequestData
                                {
                                    Authority = requestResult.Authority ?? string.Empty,
                                    Code = requestResult.Code.GetValueOrDefault(),
                                    Fee = requestResult.Fee.GetValueOrDefault(),
                                    FeeType = requestResult.FeeType ?? string.Empty,
                                    Message = requestResult.Message ?? string.Empty,
                                }
                            };
                        }
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

            return new ZarinpalRequestResultDTO(false, string.Empty, ZarinpalStatusCode.St400);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new ZarinpalRequestResultDTO(false, string.Empty, ZarinpalStatusCode.St400);
        }
    }

    public async Task<ZarinpalVerifyResultDTO> VerifyAsync(ZarinpalVerifyDTO verify)
    {
        if (!InternalExtension.IsValidZarinpalVerify(verify))
        {
            throw new ZarinpalException("لطفا تمام فیلدهای اجباری را به درستی پر کنید !!!");
        }

        try
        {
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
                                (ZarinpalStatusCode)requestResult.Code)
                            {
                                Data = new ZarinpalVerifyResultData
                                {
                                    RefId = requestResult.RefId,
                                    Fee = requestResult.Fee,
                                    FeeType = requestResult.FeeType ?? string.Empty,
                                    CardHash = requestResult.CardHash ?? string.Empty,
                                    CardPan = requestResult.CardPan ?? string.Empty,
                                    Code = requestResult.Code,
                                }
                            };
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

            return new ZarinpalVerifyResultDTO(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new ZarinpalVerifyResultDTO(false);
        }
    }

    #endregion

    #region dispose

    public void Dispose()
    {
        _httpClient?.Dispose();
        GC.SuppressFinalize(this);
    }

    #endregion
}