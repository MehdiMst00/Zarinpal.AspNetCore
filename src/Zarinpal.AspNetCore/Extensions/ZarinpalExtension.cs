namespace Zarinpal.AspNetCore.Extensions;

public static class ZarinpalExtension
{
    public static IServiceCollection AddZarinpal(this IServiceCollection services, Action<ZarinpalOptions> options)
    {
        services.Configure(options);

        var provider = services.BuildServiceProvider();
        ZarinpalOptions option = provider.GetRequiredService<IOptions<ZarinpalOptions>>().Value;

        if (!InternalExtension.IsValidMerchantId(option.MerchantId))
        {
            throw new Exception("لطفا كد 36 كاراكتري اختصاصي پذيرنده را به درستی وارد کنید !!!");
        }

        var baseUrl = BaseUrlConst.GetBaseUrl(option.ZarinpalMode);

        services.AddSingleton<IAsyncPolicy<HttpResponseMessage>>(
            ZarinpalUtilities.RetryPolicy()
        );

        services.AddTransient<PollyRetryHandler>();

        services.AddHttpClient<IZarinpalService, OriginalZarinpalService>((client) =>
        {
            client.BaseAddress = new Uri(baseUrl);
        }).AddHttpMessageHandler<PollyRetryHandler>();

        services.AddHttpClient<IAdvancedZarinpalService, AdvancedZarinpalService>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
        }).AddHttpMessageHandler<PollyRetryHandler>();

        return services;
    }

    public static bool IsValidZarinpalVerifyQueries(this HttpContext httpContext)
    {
        return httpContext.Request.Query["Status"] != "" &&
               httpContext.Request.Query["Status"].ToString().Equals("ok", StringComparison.CurrentCultureIgnoreCase) &&
               httpContext.Request.Query["Authority"] != "";
    }

    public static string? GetZarinpalAuthorityQuery(this HttpContext httpContext)
    {
        return httpContext.Request.Query["Authority"];
    }
}