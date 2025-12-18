using Polly.Retry;

namespace Zarinpal.AspNetCore.Utilities;

internal static class ZarinpalUtilities
{
    private static readonly HttpStatusCode[] InvalidStatusCodes =
    [
        HttpStatusCode.RequestTimeout,
        HttpStatusCode.InternalServerError,
        HttpStatusCode.BadGateway,
        HttpStatusCode.ServiceUnavailable,
        HttpStatusCode.GatewayTimeout
    ];

    internal static AsyncRetryPolicy<HttpResponseMessage> RetryPolicy()
    {
        return Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrResult(response => InvalidStatusCodes.Contains(response.StatusCode))
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            );
    }
}