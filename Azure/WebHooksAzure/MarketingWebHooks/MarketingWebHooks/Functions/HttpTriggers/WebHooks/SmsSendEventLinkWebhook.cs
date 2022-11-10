using MarketingWebHooks.Helpers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.Functions.HttpTriggers
{
    public class SmsSendEventLinkWebhook
    {
        private readonly ILogger _logger;
        private readonly IHttpHelper _httpHelper;

        public SmsSendEventLinkWebhook(ILoggerFactory loggerFactory, IHttpHelper httpHelper)
        {
            _logger = loggerFactory.CreateLogger<SmsSendEventLinkWebhook>();
            _httpHelper = httpHelper;
        }

        [Function("SmsSendEventLinkWebhook")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData requestData)
        {
            _logger.LogInformation("SmsSendEventLinkWebhook|Start");

            _logger.LogInformation("SmsSendEventLinkWebhook|Finish");

            return await _httpHelper.CreateFailedHttpResponseAsync(requestData, "Handled");
        }
    }
}
