using System.Text.Json.Serialization;

namespace Zarinpal.AspNetCore.DTOs;

internal class RequestDTO
{
    [JsonPropertyName("merchant_id")]
    public string? MerchantId { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("mobile")]
    public string? Mobile { get; set; }

    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("callback_url")]
    public string? VerifyCallbackUrl { get; set; }
}

internal record RequestResult(
    [property: JsonPropertyName("data")] RequestResultData? Data,
    [property: JsonPropertyName("errors")] object[]? Errors
);

internal record RequestResultData(
    [property: JsonPropertyName("code")] int Code,
    [property: JsonPropertyName("message")] string Message,
    [property: JsonPropertyName("authority")] string Authority,
    [property: JsonPropertyName("fee_type")] string FeeType,
    [property: JsonPropertyName("fee")] int Fee
);