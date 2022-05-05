using Zarinpal.AspNetCore.DTOs.Common;

namespace Zarinpal.AspNetCore.DTOs;

public class ZarinpalVerifyResultDTO
{
    public bool IsSuccessStatusCode { get; set; }

    public ulong? RefId { get; set; }

    public ZarinpalStatusCode? StatusCode { get; set; }

    public ZarinpalVerifyResultDTO(bool isSuccessStatusCode)
    {
        IsSuccessStatusCode = isSuccessStatusCode;
    }

    public ZarinpalVerifyResultDTO(bool isSuccessStatusCode, ulong? refId, ZarinpalStatusCode? statusCode)
    {
        IsSuccessStatusCode = isSuccessStatusCode;
        RefId = refId;
        StatusCode = statusCode;
    }
}