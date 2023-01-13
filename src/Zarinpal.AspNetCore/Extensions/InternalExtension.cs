namespace Zarinpal.AspNetCore.Extensions;

internal static class InternalExtension
{
    internal static bool IsValidMerchantId(string? merchantId)
    {
        return merchantId?.Length == 36;
    }

    internal static bool IsValidZarinpalRequest(ZarinpalRequestDTO request)
    {
        return request.Amount > 0 &&
                !string.IsNullOrEmpty(request.Description) &&
                !string.IsNullOrEmpty(request.VerifyCallbackUrl) &&
                (request.VerifyCallbackUrl.ToLower().StartsWith("http://") ||
                request.VerifyCallbackUrl.ToLower().StartsWith("https://"));
    }

    internal static bool IsValidZarinpalVerify(ZarinpalVerifyDTO verify)
    {
        return !string.IsNullOrEmpty(verify.Authority) && verify.Amount > 0;
    }
}
