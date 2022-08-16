namespace Zarinpal.AspNetCore.Implementations;

public class AdvancedZarinpalService : IAdvancedZarinpalService
{
    #region constrcutor

    private readonly HttpClient _httpClient;
    private readonly ZarinpalOptions _options;

    public AdvancedZarinpalService(HttpClient httpClient, IOptions<ZarinpalOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    #endregion

    #region un verified

    public async Task<ZarinpalUnVerifyDTO> UnVerifiedAsync()
    {
        // Here we on sandbox mode :)
        if (_options.ZarinpalMode != ZarinpalMode.Original)
            return new ZarinpalUnVerifyDTO(false, ZarinpalStatusCode.St400, null);

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
                    var code = Convert.ToInt32(data["code"]?.GetValue<string>() ?? "400");

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

    #region dispose

    public void Dispose()
    {
        _httpClient?.Dispose();
    }

    #endregion
}