using Polly.Retry;

namespace MarketingWebHooks.ResiliencePolicy
{
    public interface IRetryPolicy
    {
        AsyncRetryPolicy RetryPolicyHandler { get; }
    }
}
