using MarketingWebHooks.Helpers;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.Functions.HttpTriggers
{
    public class GetCampaignBroadcastStatistic
    {
        private readonly ILogger _logger;
        private readonly IMarketingService _marketingService;
        private readonly IHttpHelper _httpHelper;

        public GetCampaignBroadcastStatistic(ILoggerFactory loggerFactory, IHttpHelper httpHelper, IMarketingService marketingService)
        {
            _logger = loggerFactory.CreateLogger<GetCampaignBroadcastStatistic>();
            _httpHelper = httpHelper;
            _marketingService = marketingService;
        }

        [Function("GetCampaignBroadcastStatistic")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData requestData)
        {
            _logger.LogInformation("GetEmailStatuses|Start GetItemsWithLockProcessing");

            int photographerKey = 1;
            int eventKey = 2;
            int campaignKey = 3;
            int broadcastKey = 4;
            int campaignBroadcastKey = 5;

            var statistic = await _marketingService.GetCampaignBroadcastStatistic(photographerKey, eventKey,
                                        campaignKey, broadcastKey, campaignBroadcastKey);

            _logger.LogInformation("GetEmailStatuses|Finish GetItemsWithLockProcessing");

            return await _httpHelper.CreateSuccessfulHttpResponseAsync(requestData, statistic);
        }
    }
}
