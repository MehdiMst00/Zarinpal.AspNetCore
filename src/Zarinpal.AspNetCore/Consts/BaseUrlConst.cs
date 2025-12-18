namespace Zarinpal.AspNetCore.Consts;

internal static class BaseUrlConst
{
    private static readonly string Original = "https://payment.zarinpal.com/pg/";
    private static readonly string Sandbox = "https://sandbox.zarinpal.com/pg/";

    public static string GetBaseUrl(ZarinpalMode mode)
    {
        return mode == ZarinpalMode.Original ? Original : Sandbox;
    }
}