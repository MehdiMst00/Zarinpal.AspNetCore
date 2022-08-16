namespace Zarinpal.AspNetCore.Utilities;

internal static class ZarinpalUtilities
{
    private static readonly List<HttpStatusCode> invalidStatusCode = new()
    {
        HttpStatusCode.BadGateway,
        HttpStatusCode.InternalServerError,
        HttpStatusCode.BadRequest,
        HttpStatusCode.RequestTimeout,
        HttpStatusCode.Forbidden,
        HttpStatusCode.GatewayTimeout
    };

    internal static IAsyncPolicy<HttpResponseMessage> RetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => invalidStatusCode.Contains(msg.StatusCode))
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}