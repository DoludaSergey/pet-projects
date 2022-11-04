using Polly.Retry;

namespace MarketingWebHooks.ResiliencePolicy
{
    public interface ICosmosRetryPolicy
    {
        AsyncRetryPolicy RetryPolicyHandler { get; }
    }
}
