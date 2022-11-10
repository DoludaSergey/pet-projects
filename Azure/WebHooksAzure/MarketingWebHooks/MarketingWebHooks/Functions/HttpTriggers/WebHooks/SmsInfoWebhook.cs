using MarketingWebHooks.Helpers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.Functions.HttpTriggers
{
    public class SmsInfoWebhook
    {
        private readonly ILogger _logger;
        private readonly IHttpHelper _httpHelper;

        public SmsInfoWebhook(ILoggerFactory loggerFactory, IHttpHelper httpHelper)
        {
            _logger = loggerFactory.CreateLogger<SmsInfoWebhook>();
            _httpHelper = httpHelper;
        }

        [Function("SmsInfoWebhook")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData requestData)
        {
            _logger.LogInformation("SmsInfoWebhook|Start");

            _logger.LogInformation("SmsInfoWebhook|Finish");

            return await _httpHelper.CreateFailedHttpResponseAsync(requestData, "Handled");
        }
    }
}
