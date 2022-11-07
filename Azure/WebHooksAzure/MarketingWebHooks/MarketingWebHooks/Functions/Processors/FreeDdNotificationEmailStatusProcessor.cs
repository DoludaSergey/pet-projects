using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MarketingWebHooks.Functions.Processors
{
    public class FreeDdNotificationEmailStatusProcessor
    {
        private readonly ILogger _logger;
        private readonly IFreeDdNotificationRepository _freeDdNotificationRepository;

        public FreeDdNotificationEmailStatusProcessor(ILoggerFactory loggerFactory, IFreeDdNotificationRepository repository)
        {
            _logger = loggerFactory.CreateLogger<FreeDdNotificationEmailStatusProcessor>();
            _freeDdNotificationRepository = repository;
        }

        [Function("FreeDdNotificationEmailStatusProcessor")]
        public async Task Run([ServiceBusTrigger(AzurServiceBusQueueMessageService.SERVER_BUS_QUEUE_FREE_DD_NOTIFICATION_EMAIL_STATUS_PROCESS, Connection = "SERVER_BUS_QUEUE_CON_STR")] string myQueueItem)
        {
            _logger.LogInformation($"FreeDdNotificationEmailStatusProcessor: Started processing message: {myQueueItem}");

            try
            {
                FreeDdEmailNotificationStatus webhookModel = JsonSerializer.Deserialize<FreeDdEmailNotificationStatus>(myQueueItem);

                _logger.LogInformation($"FreeDdNotificationEmailStatusProcessor: the queue message was Deserialized");

                await _freeDdNotificationRepository.AddAsync(webhookModel);

                _logger.LogInformation($"FreeDdNotificationEmailStatusProcessor: Finished ");
            }
            catch (Exception ex)
            {
                _logger.LogError($"FreeDdNotificationEmailStatusProcessor: Failed. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");

                throw;
            }

            _logger.LogInformation("FreeDdNotificationEmailStatusProcessor: Finished");
        }
    }
}
