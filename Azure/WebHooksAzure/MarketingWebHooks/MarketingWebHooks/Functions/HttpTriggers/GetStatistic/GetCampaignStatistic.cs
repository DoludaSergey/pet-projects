using MarketingWebHooks.Helpers;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.Functions.HttpTriggers
{
    public class GetCampaignStatistic
    {
        private readonly ILogger _logger;
        private readonly IMarketingService _marketingService;
        private readonly IHttpHelper _httpHelper;

        public GetCampaignStatistic(ILoggerFactory loggerFactory, IHttpHelper httpHelper, IMarketingService marketingService)
        {
            _logger = loggerFactory.CreateLogger<GetCampaignStatistic>();
            _httpHelper = httpHelper;
            _marketingService = marketingService;
        }

        [Function("GetCampaignStatistic")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData requestData)
        {
            _logger.LogInformation("GetCampaignStatistic|Start GetCampaignStatistic");

            int photographerKey = 1;
            int eventKey = 2;
            int campaignKey = 3;

            var statistic = await _marketingService.GetCampaignStatisticAsync(photographerKey, eventKey, campaignKey);

            _logger.LogInformation("GetCampaignStatistic|Finish GetCampaignStatistic");

            return await _httpHelper.CreateSuccessfulHttpResponseAsync(requestData, statistic);
        }
    }
}
