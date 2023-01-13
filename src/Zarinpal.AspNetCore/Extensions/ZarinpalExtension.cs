namespace Zarinpal.AspNetCore.Extensions;

public static class ZarinpalExtension
{
    private static readonly string baseUrl = "https://api.zarinpal.com/pg/";
    private static readonly string sandboxUrl = "https://sandbox.zarinpal.com/pg/";

    public static IServiceCollection AddZarinpal(this IServiceCollection services, Action<ZarinpalOptions> options)
    {
        services.Configure<ZarinpalOptions>(options);

        var provider = services.BuildServiceProvider();
        ZarinpalOptions? option = provider.GetService<IOptions<ZarinpalOptions>>()?.Value;

        if (option == null)
        {
            throw new ZarinpalException("لطفا تنظیمات زرین پال را در (Startup یا Program) برنامه به درستی وارد کنید !!!");
        }

        if (!InternalExtension.IsValidMerchantId(option.MerchantId))
        {
            throw new Exception("لطفا كد 36 كاراكتري اختصاصي پذيرنده را به درستی وارد کنید !!!");
        }

        if (option.ZarinpalMode == ZarinpalMode.Sandbox) // Sandbox
        {
            services.AddHttpClient<IZarinpalService, SandboxZarinpalService>((client) =>
            {
                client.BaseAddress = new Uri(sandboxUrl);
            }).AddPolicyHandler(ZarinpalUtilities.RetryPolicy());
        }
        else // Original
        {
            services.AddHttpClient<IZarinpalService, OriginalZarinpalService>((client) =>
            {
                client.BaseAddress = new Uri(baseUrl);
            }).AddPolicyHandler(ZarinpalUtilities.RetryPolicy());
        }

        if (option.UseAdvanced)
        {
            services.AddHttpClient<IAdvancedZarinpalService, AdvancedZarinpalService>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            }).AddPolicyHandler(ZarinpalUtilities.RetryPolicy());
        }

        return services;
    }

    public static bool IsValidZarinpalVerifyQueries(this HttpContext httpContext)
    {
        return httpContext.Request.Query["Status"] != "" &&
               httpContext.Request.Query["Status"].ToString().ToLower() == "ok"
               && httpContext.Request.Query["Authority"] != "";
    }

    public static string GetZarinpalAuthorityQuery(this HttpContext httpContext)
    {
        return httpContext.Request.Query["Authority"];
    }

    public static int TomanToRial(this int toman)
    {
        return Convert.ToInt32(Convert.ToString(toman) + "0");
    }
}