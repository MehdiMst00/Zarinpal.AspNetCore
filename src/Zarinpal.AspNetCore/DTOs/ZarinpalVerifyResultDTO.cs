namespace Zarinpal.AspNetCore.DTOs;

public class ZarinpalVerifyResultDTO
{
    public bool IsSuccessStatusCode { get; set; }

    public ulong? RefId { get; set; }

    public ZarinpalStatusCode? StatusCode { get; set; }

    public ZarinpalVerifyResultData? Data { get; set; }

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

public class ZarinpalVerifyResultData
{
    public int Code { get; set; }
    public ulong RefId { get; set; }
    public string CardPan { get; set; } = null!;
    public string CardHash { get; set; } = null!;
    public string FeeType { get; set; } = null!;
    public int Fee { get; set; }
}