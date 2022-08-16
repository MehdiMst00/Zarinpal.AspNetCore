namespace Zarinpal.AspNetCore.Extensions;

public static class ZarinpalExtension
{
    private static readonly string baseUrl = "https://api.zarinpal.com/pg/";
    private static readonly string sandboxUrl = "https://sandbox.zarinpal.com/pg/";

    public static IServiceCollection AddZarinpal(this IServiceCollection services, Action<ZarinpalOptions> options, bool useAdvanced = false)
    {
        services.Configure<ZarinpalOptions>(options);

        services.AddHttpClient<IZarinpalService, ZarinpalService>((provider, client) =>
        {
            var option = provider.GetService<IOptions<ZarinpalOptions>>();
            if (option == null) return;

            client.BaseAddress = option.Value.ZarinpalMode == ZarinpalMode.Original ?
                new Uri(baseUrl) :
                new Uri(sandboxUrl);

        }).AddPolicyHandler(ZarinpalUtilities.RetryPolicy());

        if (useAdvanced)
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