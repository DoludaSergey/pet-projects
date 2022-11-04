using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace MarketingWebHooks.ResiliencePolicy
{
    public class RetryPolicy : IRetryPolicy
    {
        public ILogger _logger;
        private readonly int MaxRetries = 3;
        private readonly AsyncRetryPolicy _retryPolicy;

        public AsyncRetryPolicy RetryPolicyHandler { get { return _retryPolicy; } }

        public RetryPolicy(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RetryPolicy>();

            _retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(retryCount: MaxRetries, retryAttempt => TimeSpan.FromSeconds(1),
                onRetry: (exception, time, context) =>
                {
                    _logger.LogError($"RetryPolicy: Exception Message: {exception.Message} : Stack: {exception.StackTrace}");
                });
        }
    }
}
