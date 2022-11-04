using MarketingWebHooks.DataAcesLayer;
using MarketingWebHooks.Entities;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MarketingWebHooks.Functions.Processors
{
    public class CampaignBroadcastStatusProcessor
    {
        private readonly ILogger _logger;
        private readonly ICampaignBroadcastRepository _campaignBroadcastRepository;

        public CampaignBroadcastStatusProcessor(ILoggerFactory loggerFactory, ICampaignBroadcastRepository campaignBroadcastRepository)
        {
            _logger = loggerFactory.CreateLogger<CampaignBroadcastStatusProcessor>();
            _campaignBroadcastRepository = campaignBroadcastRepository;
        }

        [Function("CampaignBroadcastStatusProcessor")]
        public async Task Run([ServiceBusTrigger(AzurServiceBusQueueMessageService.SERVER_BUS_QUEUE_CAMPAIGN_BROADCAST_EMAIL_STATUS_PROCESS, Connection = "SERVER_BUS_QUEUE_CON_STR")] string myQueueItem)
        {
            _logger.LogInformation($"CampaignBroadcastStatusProcessor: Started processing message: {myQueueItem}");

            try
            {
                CampaignBroadcast campaignBroadcast = JsonSerializer.Deserialize<CampaignBroadcast>(myQueueItem);

                _logger.LogInformation($"CampaignBroadcastStatusProcessor: the queue message was Deserialized");

                // Get CampaignBroadcst from db
                var campaignBroadcastFromBd = await _campaignBroadcastRepository.GetByIdAsnc(campaignBroadcast.Id);
                //CampaignBroadcast? campaignBroadcastFromBd = null;


                // If CampaignBroadcst exists - update it
                if (campaignBroadcastFromBd is not null)
                {
                    campaignBroadcastFromBd.Status = campaignBroadcast.Status;
                    campaignBroadcastFromBd.SetStatus();

                    //Save CampaignBroadcst into db
                    await _campaignBroadcastRepository.UpdateAsync(campaignBroadcastFromBd);
                }
                // If does not exist
                else
                {
                    campaignBroadcast.SetStatus();

                    // Add CampaignBroadcst into db
                    await _campaignBroadcastRepository.AddAsync(campaignBroadcast);
                }                

                _logger.LogInformation($"CampaignBroadcastStatusProcessor: Finished ");
            }
            catch (Exception ex)
            {
                _logger.LogError($"CampaignBroadcastStatusProcessor: Failed. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");

                throw;
            }

            _logger.LogInformation("GenerateThumbnailImagesProcessor: Finished");
        }
    }
}
