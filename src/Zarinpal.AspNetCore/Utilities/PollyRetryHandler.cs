namespace Zarinpal.AspNetCore.Utilities;

internal sealed class PollyRetryHandler : DelegatingHandler
{
    private readonly IAsyncPolicy<HttpResponseMessage> _policy;

    public PollyRetryHandler(IAsyncPolicy<HttpResponseMessage> policy)
    {
        _policy = policy;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        return _policy.ExecuteAsync(
            ct => base.SendAsync(request, ct),
            cancellationToken
        );
    }
}
