namespace Zarinpal.AspNetCore.Implementations;

public class SandboxZarinpalService : IZarinpalService
{
    #region constructor

    private readonly ILogger<SandboxZarinpalService> _logger;
    private readonly HttpClient _httpClient;
    private readonly ZarinpalOptions _zarinpalOptions;

    public SandboxZarinpalService(ILogger<SandboxZarinpalService> logger,
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
