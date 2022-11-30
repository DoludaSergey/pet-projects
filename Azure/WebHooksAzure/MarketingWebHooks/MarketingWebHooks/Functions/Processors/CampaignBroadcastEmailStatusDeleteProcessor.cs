using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MarketingWebHooks.Functions.Processors
{
    public class CampaignBroadcastEmailStatusDeleteProcessor
    {
        private readonly ILogger _logger;
        private readonly ICampaignBroadcastEmailStatusExtendedRepository _campaignBroadcastRepository;

        public CampaignBroadcastEmailStatusDeleteProcessor(ILoggerFactory loggerFactory, 
            ICampaignBroadcastEmailStatusExtendedRepository campaignBroadcastRepository)
        {
            _logger = loggerFactory.CreateLogger<CampaignBroadcastEmailStatusDeleteProcessor>();
            _campaignBroadcastRepository = campaignBroadcastRepository;
        }

        [Function("CampaignBroadcastEmailStatusDeleteProcessor")]
        public async Task Run([ServiceBusTrigger(AzurServiceBusQueueMessageService.SERVER_BUS_QUEUE_CAMPAIGN_BROADCAST_EMAIL_STATUS_DELETE_PROCESS,
            Connection = "SERVER_BUS_QUEUE_CON_STR")] string myQueueItem)
        {
            _logger.LogInformation($"CampaignBroadcastEmailStatusDeleteProcessor: Started processing message: {myQueueItem}");

            try
            {
                string campaignBroadcastEmailStatusId = JsonSerializer.Deserialize<string>(myQueueItem);

                _logger.LogInformation($"CampaignBroadcastEmailStatusDeleteProcessor: the queue message was Deserialized");

                await _campaignBroadcastRepository.RemoveAsync(campaignBroadcastEmailStatusId);

                _logger.LogInformation($"CampaignBroadcastEmailStatusDeleteProcessor: Finished ");
            }
            catch (Exception ex)
            {
                _logger.LogError($"CampaignBroadcastEmailStatusDeleteProcessor: Failed. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");

                throw;
            }

            _logger.LogInformation("CampaignBroadcastEmailStatusDeleteProcessor: Finished");
        }
    }
}
