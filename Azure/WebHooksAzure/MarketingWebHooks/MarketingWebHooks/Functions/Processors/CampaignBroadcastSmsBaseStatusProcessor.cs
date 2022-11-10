using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities.CampaignBroadcast;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MarketingWebHooks.Functions.Processors
{
    public class CampaignBroadcastSmsBaseStatusProcessor
    {
        private readonly ILogger _logger;
        private readonly ICampaignBroadcastSmsStatusRepository _campaignBroadcastRepository;

        public CampaignBroadcastSmsBaseStatusProcessor(ILoggerFactory loggerFactory, ICampaignBroadcastSmsStatusRepository campaignBroadcastRepository)
        {
            _logger = loggerFactory.CreateLogger<CampaignBroadcastSmsBaseStatusProcessor>();
            _campaignBroadcastRepository = campaignBroadcastRepository;
        }

        [Function("CampaignBroadcastSmsBaseStatusProcessor")]
        public async Task Run([ServiceBusTrigger(AzurServiceBusQueueMessageService.SERVER_BUS_QUEUE_CAMPAIGN_BROADCAST_SMS_STATUS_PROCESS, Connection = "SERVER_BUS_QUEUE_CON_STR")] string myQueueItem)
        {
            _logger.LogInformation($"CampaignBroadcastEmailBaseStatusProcessor: Started processing message: {myQueueItem}");

            try
            {
                CampaignBroadcastSmsStatus campaignBroadcastSmsStatus = JsonSerializer.Deserialize<CampaignBroadcastSmsStatus>(myQueueItem);

                _logger.LogInformation($"CampaignBroadcastSmsBaseStatusProcessor: the queue message was Deserialized");

                // Add CampaignBroadcst into db
                await _campaignBroadcastRepository.AddAsync(campaignBroadcastSmsStatus);

                _logger.LogInformation($"CampaignBroadcastSmsBaseStatusProcessor: Finished ");
            }
            catch (Exception ex)
            {
                _logger.LogError($"CampaignBroadcastSmsBaseStatusProcessor: Failed. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");

                throw;
            }

            _logger.LogInformation("CampaignBroadcastSmsBaseStatusProcessor: Finished");
        }
    }
}
