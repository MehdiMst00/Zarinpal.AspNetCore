using System.Text.Json.Serialization;

namespace Zarinpal.AspNetCore.Models.Internal.Original;

internal class VerifyDTO
{
    [JsonPropertyName("merchant_id")]
    public string? MerchantId { get; set; }

    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("authority")]
    public string? Authority { get; set; }
}

internal class VerifyResultData
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("ref_id")]
    public ulong RefId { get; set; }

    [JsonPropertyName("card_pan")]
    public string? CardPan { get; set; }

    [JsonPropertyName("card_hash")]
    public string? CardHash { get; set; }

    [JsonPropertyName("fee_type")]
    public string? FeeType { get; set; }

    [JsonPropertyName("fee")]
    public int Fee { get; set; }
}