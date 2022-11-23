using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MarketingWebHooks.Functions.Processors
{
    public class CampaignBroadcastEmailStatusProcessor
    {
        private readonly ILogger _logger;
        private readonly ICampaignBroadcastEmailStatusExtendedRepository _campaignBroadcastRepository;

        public CampaignBroadcastEmailStatusProcessor(ILoggerFactory loggerFactory, 
            ICampaignBroadcastEmailStatusExtendedRepository campaignBroadcastRepository)
        {
            _logger = loggerFactory.CreateLogger<CampaignBroadcastEmailStatusProcessor>();
            _campaignBroadcastRepository = campaignBroadcastRepository;
        }

        [Function("CampaignBroadcastEmailStatusProcessor")]
        public async Task Run([ServiceBusTrigger(AzurServiceBusQueueMessageService.SERVER_BUS_QUEUE_CAMPAIGN_BROADCAST_EMAIL_STATUS_PROCESS,
            Connection = "SERVER_BUS_QUEUE_CON_STR")] string myQueueItem)
        {
            _logger.LogInformation($"CampaignBroadcastEmailStatusProcessor: Started processing message: {myQueueItem}");

            try
            {
                CampaignBroadcastEmailStatusExtended campaignBroadcast = JsonSerializer.Deserialize<CampaignBroadcastEmailStatusExtended>(myQueueItem);

                _logger.LogInformation($"CampaignBroadcastEmailStatusProcessor: the queue message was Deserialized");

                // Add CampaignBroadcst into db
                await _campaignBroadcastRepository.AddAsync(campaignBroadcast);

                _logger.LogInformation($"CampaignBroadcastEmailStatusProcessor: Finished ");
            }
            catch (Exception ex)
            {
                _logger.LogError($"CampaignBroadcastEmailStatusProcessor: Failed. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");

                throw;
            }

            _logger.LogInformation("CampaignBroadcastEmailStatusProcessor: Finished");
        }
    }
}
