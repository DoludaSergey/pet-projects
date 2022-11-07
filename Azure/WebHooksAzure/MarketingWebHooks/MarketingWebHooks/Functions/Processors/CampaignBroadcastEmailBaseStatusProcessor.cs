using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MarketingWebHooks.Functions.Processors
{
    public class CampaignBroadcastEmailBaseStatusProcessor
    {
        private readonly ILogger _logger;
        private readonly ICampaignBroadcastBaseRepository _campaignBroadcastRepository;

        public CampaignBroadcastEmailBaseStatusProcessor(ILoggerFactory loggerFactory, ICampaignBroadcastBaseRepository campaignBroadcastRepository)
        {
            _logger = loggerFactory.CreateLogger<CampaignBroadcastEmailBaseStatusProcessor>();
            _campaignBroadcastRepository = campaignBroadcastRepository;
        }

        [Function("CampaignBroadcastEmailBaseStatusProcessor")]
        public async Task Run([ServiceBusTrigger(AzurServiceBusQueueMessageService.SERVER_BUS_QUEUE_CAMPAIGN_BROADCAST_EMAIL_BASE_STATUS_PROCESS, Connection = "SERVER_BUS_QUEUE_CON_STR")] string myQueueItem)
        {
            _logger.LogInformation($"CampaignBroadcastEmailBaseStatusProcessor: Started processing message: {myQueueItem}");

            try
            {
                CampaignBroadcastEmailStatus campaignBroadcast = JsonSerializer.Deserialize<CampaignBroadcastEmailStatus>(myQueueItem);

                _logger.LogInformation($"CampaignBroadcastEmailBaseStatusProcessor: the queue message was Deserialized");

                // Add CampaignBroadcst into db
                await _campaignBroadcastRepository.AddAsync(campaignBroadcast);

                _logger.LogInformation($"CampaignBroadcastEmailBaseStatusProcessor: Finished ");
            }
            catch (Exception ex)
            {
                _logger.LogError($"CampaignBroadcastEmailBaseStatusProcessor: Failed. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");

                throw;
            }

            _logger.LogInformation("CampaignBroadcastEmailBaseStatusProcessor: Finished");
        }
    }
}
