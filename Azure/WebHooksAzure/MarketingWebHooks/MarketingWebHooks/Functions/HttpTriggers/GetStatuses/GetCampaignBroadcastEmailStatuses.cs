using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities;
using MarketingWebHooks.Helpers;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.Functions.HttpTriggers
{
    public class GetCampaignBroadcastEmailStatuses
    {
        private readonly ILogger _logger;
        private readonly ICampaignBroadcastEmailStatusRepository _campaignBroadcastRepository;
        private readonly IHttpHelper _httpHelper;

        public GetCampaignBroadcastEmailStatuses(ILoggerFactory loggerFactory, ICampaignBroadcastEmailStatusRepository campaignBroadcastRepository, IHttpHelper httpHelper)
        {
            _logger = loggerFactory.CreateLogger<GetCampaignBroadcastEmailStatuses>();
            _campaignBroadcastRepository = campaignBroadcastRepository;
            _httpHelper = httpHelper;
        }

        [Function("GetCampaignBroadcastEmailStatuses")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData requestData)
        {
            _logger.LogInformation("GetEmailStatuses|Start GetItemsWithLockProcessing");            

            List<CampaignBroadcastEmailStatus>? itemsToProcess = await ItemsLockProcessor
                .GetItemsWithLockProcessing(_campaignBroadcastRepository, _logger);

            _logger.LogInformation("GetEmailStatuses|Finish GetItemsWithLockProcessing");

            if (itemsToProcess is null)
            {
                _logger.LogInformation("GetEmailStatuses|itemsToProcess is null");

                itemsToProcess = new();                

                return await _httpHelper.CreateFailedHttpResponseAsync(requestData, itemsToProcess);
            }

            return await _httpHelper.CreateSuccessfulHttpResponseAsync(requestData, itemsToProcess);
        }
    }
}
