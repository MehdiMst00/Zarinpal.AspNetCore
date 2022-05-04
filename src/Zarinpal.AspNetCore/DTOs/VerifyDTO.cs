using System.Text.Json.Serialization;

namespace Zarinpal.AspNetCore.DTOs;

internal class VerifyDTO
{
    [JsonPropertyName("merchant_id")]
    public string? MerchantId { get; set; }

    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("authority")]
    public string? Authority { get; set; }
}

internal record VerifyResult(
    [property: JsonPropertyName("data")] VerifyResultData? Data,
    [property: JsonPropertyName("errors")] object[]? Errors
);

internal record VerifyResultData(
    [property: JsonPropertyName("code")] int Code,
    [property: JsonPropertyName("ref_id")] ulong RefId,
    [property: JsonPropertyName("card_pan")] string CardPan,
    [property: JsonPropertyName("card_hash")] string CardHash,
    [property: JsonPropertyName("fee_type")] string FeeType,
    [property: JsonPropertyName("fee")] int Fee
);