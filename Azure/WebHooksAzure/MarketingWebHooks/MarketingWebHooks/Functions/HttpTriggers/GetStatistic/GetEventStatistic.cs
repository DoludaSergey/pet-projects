using MarketingWebHooks.Helpers;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.Functions.HttpTriggers
{
    public class GetEventStatistic
    {
        private readonly ILogger _logger;
        private readonly IMarketingService _marketingService;
        private readonly IHttpHelper _httpHelper;

        public GetEventStatistic(ILoggerFactory loggerFactory, IHttpHelper httpHelper, IMarketingService marketingService)
        {
            _logger = loggerFactory.CreateLogger<GetEventStatistic>();
            _httpHelper = httpHelper;
            _marketingService = marketingService;
        }

        [Function("GetEventStatistic")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData requestData)
        {
            _logger.LogInformation("GetEventStatistic|Start");

            int photographerKey = 1;
            int eventKey = 2;

            var statistic = await _marketingService.GetEventStatisticAsync(photographerKey, eventKey);

            _logger.LogInformation("GetEventStatistic|Finish");

            return await _httpHelper.CreateSuccessfulHttpResponseAsync(requestData, statistic);
        }
    }
}
