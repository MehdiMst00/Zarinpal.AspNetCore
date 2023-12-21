namespace Zarinpal.AspNetCore.Models.Internal.Sandbox;

internal class SandboxVerifyDTO
{
    [JsonPropertyName("MerchantID")]
    public string? MerchantId { get; set; }

    [JsonPropertyName("Amount")]
    public int Amount { get; set; }

    [JsonPropertyName("Authority")]
    public string? Authority { get; set; }
}

internal record SandboxVerifyResult(int Status, ulong RefID);