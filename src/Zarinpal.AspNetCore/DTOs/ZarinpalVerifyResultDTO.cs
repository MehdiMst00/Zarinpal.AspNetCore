namespace Zarinpal.AspNetCore.DTOs;

public class ZarinpalVerifyResultDTO
{
    public bool IsSuccessStatusCode { get; set; }

    public ulong RefId { get; set; }

    public ZarinpalVerifyResultDTO(bool isSuccessStatusCode)
    {
        IsSuccessStatusCode = isSuccessStatusCode;
    }

    public ZarinpalVerifyResultDTO(bool isSuccessStatusCode, ulong refId)
    {
        IsSuccessStatusCode = isSuccessStatusCode;
        RefId = refId;
    }
}