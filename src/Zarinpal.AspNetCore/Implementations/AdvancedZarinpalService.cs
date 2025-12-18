namespace Zarinpal.AspNetCore.Implementations;

public class AdvancedZarinpalService : IAdvancedZarinpalService
{
    #region Ctor

    private readonly HttpClient _httpClient;
    private readonly ZarinpalOptions _options;

    public AdvancedZarinpalService(HttpClient httpClient, IOptions<ZarinpalOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    #endregion

    #region UnVerified

    public async Task<ZarinpalUnVerifyDTO> UnVerifiedAsync()
    {
        // Original api
        var response = await _httpClient.PostAsJsonAsync("v4/payment/unVerified.json", new
        {
            merchant_id = _options.MerchantId
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
                    var code = data["code"]?.GetValue<int?>() ?? 400;

                    var authorities =
                        data["authorities"]?.AsArray().Deserialize<List<ZarinpalUnVerifyAuthority>>();

                    return new ZarinpalUnVerifyDTO(response.IsSuccessStatusCode,
                        (ZarinpalStatusCode)code, authorities);
                }
                else
                {
                    // we have error here
                    var errors = jsonObjectResponse["errors"] as JsonObject;
                    if (errors?.Count > 0)
                    {
                        var code = errors["code"]?.GetValue<int>();
                        if (code != null)
                            return new ZarinpalUnVerifyDTO(false, (ZarinpalStatusCode)code);
                    }
                }
            }
        }

        return new ZarinpalUnVerifyDTO(false, ZarinpalStatusCode.St400);
    }

    #endregion

    #region Dispose

    public void Dispose()
    {
        _httpClient?.Dispose();
        GC.SuppressFinalize(this);
    }

    #endregion
}