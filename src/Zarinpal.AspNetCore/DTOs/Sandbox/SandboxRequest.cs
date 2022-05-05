using System.Text.Json.Serialization;

namespace Zarinpal.AspNetCore.DTOs.Sandbox;

internal class SandboxRequest
{
    [JsonPropertyName("MerchantID")]
    public string? MerchantID { get; set; }

    [JsonPropertyName("Amount")]
    public int Amount { get; set; }

    [JsonPropertyName("CallbackURL")]
    public string? CallbackURL { get; set; }

    [JsonPropertyName("Description")]
    public string? Description { get; set; }
}

internal record SandboxRequestResult(int Status, string Authority);