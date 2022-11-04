using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace MarketingWebHooks.ResiliencePolicy
{
    public sealed class CosmosRetryPolicy : ICosmosRetryPolicy
    {
        public ILogger _logger;
        private readonly int MaxRetries = 3;
        private readonly AsyncRetryPolicy _retryPolicy;

        public AsyncRetryPolicy RetryPolicyHandler { get { return _retryPolicy; } }

        public CosmosRetryPolicy(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RetryPolicy>();

            _retryPolicy = Policy
                .Handle<CosmosException>(ex => PolicyStatusCodes.IsExceptionRetriable(ex))
                .WaitAndRetryAsync(
                retryCount: MaxRetries,
                sleepDurationProvider: (retryCount, exception) =>
                {
                    return TimeSpan.FromSeconds(1 * retryCount);
                },
                onRetry: (exception, time, context) =>
                {
                    _logger.LogError($"CosmosRetryPolicy|onRetry: Exception Message: {exception.Message} : Stack: {exception.StackTrace}");
                });
        }
    }
}
