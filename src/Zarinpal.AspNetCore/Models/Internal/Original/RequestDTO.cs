﻿namespace Zarinpal.AspNetCore.Models.Internal.Original;

internal class RequestDTO
{
    [JsonPropertyName("merchant_id")]
    public string? MerchantId { get; set; }

    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("callback_url")]
    public string? VerifyCallbackUrl { get; set; }

    [JsonPropertyName("metadata")]
    public RequestMetadataDTO? Metadata { get; set; }

    [JsonPropertyName("wages")]
    public List<ZarinpalWages>? Wages { get; set; }
}

internal class RequestMetadataDTO
{
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("mobile")]
    public string? Mobile { get; set; }

    [JsonPropertyName("order_id")]
    public string? OrderId { get; set; }

    [JsonPropertyName("card_pan")]
    public string? CardPan { get; set; }
}

internal class RequestResultData
{
    [JsonPropertyName("code")]
    public int? Code { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("authority")]
    public string? Authority { get; set; }

    [JsonPropertyName("fee_type")]
    public string? FeeType { get; set; }

    [JsonPropertyName("fee")]
    public int? Fee { get; set; }
}