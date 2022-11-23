using MarketingWebHooks.Entities;
using MarketingWebHooks.Enums;
using MarketingWebHooks.Models;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.Functions.Processors
{
    public class CampaignBroadcastStatusAddedProcessor
    {
        private readonly ILogger _logger;
        private readonly IMarketingService _marketingService;

        public CampaignBroadcastStatusAddedProcessor(ILoggerFactory loggerFactory, IMarketingService marketingService)
        {
            _logger = loggerFactory.CreateLogger<CampaignBroadcastStatusAddedProcessor>();
            _marketingService = marketingService;
        }

        [Function("CampaignBroadcastStatusAddedProcessor")]
        public async Task Run([CosmosDBTrigger(
            databaseName: "WebHook",
            collectionName: "CampaignBroadcastEmailStatusesExtended",
            ConnectionStringSetting = "COSMOS_CONNECTION",
            LeaseCollectionName = "CampaignBroadcastEmailStatusesExtended-leases",
            CreateLeaseCollectionIfNotExists = true)] IReadOnlyList<CampaignBroadcastEmailStatusExtended> inputItems)
        {
            try
            {
                if (inputItems != null && inputItems.Count > 0)
                {
                    _logger.LogInformation("CampaignBroadcastStatusAddedProcessor|Documents modified: " + inputItems.Count);

                    foreach (var item in inputItems)
                    {
                        MarketingStatisticStatus status = MarketingStatisticStatusHelper.FromString(item.Status);

                        var marketingStatisticModel = new MarketingStatisticModel(item.PhotographerKey, item.EventKey, item.CampaignKey,
                            item.BroadcastKey, item.CampaignBroadcastKey, status, item.CreationDate);

                        _logger.LogInformation($"CampaignBroadcastStatusAddedProcessor|Started StatusProcess {item.Status} CampaignBroadcastKey - {marketingStatisticModel.CampaignBroadcastKey}");

                        await _marketingService.StatusProcess(marketingStatisticModel);

                        _logger.LogInformation($"CampaignBroadcastStatusAddedProcessor|Finished StatusProcess {item.Status} CampaignBroadcastKey - {marketingStatisticModel.CampaignBroadcastKey}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
