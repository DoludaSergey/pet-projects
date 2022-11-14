using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities.CampaignBroadcast;
using MarketingWebHooks.Helpers;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.Functions.HttpTriggers
{
    public class GetCampaignBroadcastSmsStatuses
    {
        private readonly ILogger _logger;
        private readonly ICampaignBroadcastSmsStatusRepository _campaignBroadcastRepository;
        private readonly IHttpHelper _httpHelper;

        public GetCampaignBroadcastSmsStatuses(ILoggerFactory loggerFactory, ICampaignBroadcastSmsStatusRepository campaignBroadcastRepository, IHttpHelper httpHelper)
        {
            _logger = loggerFactory.CreateLogger<GetCampaignBroadcastSmsStatuses>();
            _campaignBroadcastRepository = campaignBroadcastRepository;
            _httpHelper = httpHelper;
        }

        [Function("GetCampaignBroadcastSmsStatuses")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData requestData)
        {
            _logger.LogInformation("GetCampaignBroadcastSmsStatuses|Start GetItemsWithLockProcessing");

            List<CampaignBroadcastSmsStatus>? itemsToProcess = await ItemsLockProcessor
                                                .GetItemsWithLockProcessing(_campaignBroadcastRepository, _logger);

            _logger.LogInformation("GetCampaignBroadcastSmsStatuses|Finish GetItemsWithLockProcessing");

            if (itemsToProcess is null)
            {
                _logger.LogInformation("GetCampaignBroadcastSmsStatuses|itemsToProcess is null");

                itemsToProcess = new();

                return await _httpHelper.CreateFailedHttpResponseAsync(requestData, itemsToProcess);
            }

            return await _httpHelper.CreateSuccessfulHttpResponseAsync(requestData, itemsToProcess);
        }
    }
}
