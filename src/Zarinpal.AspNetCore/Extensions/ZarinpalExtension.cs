using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Zarinpal.AspNetCore.Implementations;
using Zarinpal.AspNetCore.Interfaces;
using Zarinpal.AspNetCore.Models;
using Zarinpal.AspNetCore.Utilities;

namespace Zarinpal.AspNetCore.Extensions;

public static class ZarinpalExtension
{
    public static IServiceCollection AddZarinpal(this IServiceCollection services, Action<ZarinpalOptions> options)
    {
        services.Configure<ZarinpalOptions>(options);
        services.AddHttpClient<IZarinpalService, ZarinpalService>((provider, client) =>
        {
            var option = provider.GetService<IOptions<ZarinpalOptions>>();
            if (option == null) return;

            client.BaseAddress = option.Value.ZarinpalMode == ZarinpalMode.Original ?
                new Uri("https://api.zarinpal.com/pg/") :
                new Uri("https://sandbox.zarinpal.com/pg/");

        }).AddPolicyHandler(ZarinpalUtilities.RetryPolicy());

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