using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities.FreeDdNotification;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MarketingWebHooks.Functions.Processors
{
    public class FreeDdNotificationSmsStatusProcessor
    {
        private readonly ILogger _logger;
        private readonly IFreeDdSmsNotificationRepository _freeDdNotificationRepository;

        public FreeDdNotificationSmsStatusProcessor(ILoggerFactory loggerFactory, IFreeDdSmsNotificationRepository repository)
        {
            _logger = loggerFactory.CreateLogger<FreeDdNotificationSmsStatusProcessor>();
            _freeDdNotificationRepository = repository;
        }

        [Function("FreeDdNotificationSmsStatusProcessor")]
        public async Task Run([ServiceBusTrigger(AzurServiceBusQueueMessageService.SERVER_BUS_QUEUE_FREE_DD_NOTIFICATION_SMS_STATUS_PROCESS, Connection = "SERVER_BUS_QUEUE_CON_STR")] string myQueueItem)
        {
            _logger.LogInformation($"FreeDdNotificationSmsStatusProcessor: Started processing message: {myQueueItem}");

            try
            {
                FreeDdSmsNotificationStatus webhookModel = JsonSerializer.Deserialize<FreeDdSmsNotificationStatus>(myQueueItem);

                _logger.LogInformation($"FreeDdNotificationSmsStatusProcessor: the queue message was Deserialized");

                await _freeDdNotificationRepository.AddAsync(webhookModel);

                _logger.LogInformation($"FreeDdNotificationSmsStatusProcessor: Finished ");
            }
            catch (Exception ex)
            {
                _logger.LogError($"FreeDdNotificationSmsStatusProcessor: Failed. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");

                throw;
            }

            _logger.LogInformation("FreeDdNotificationSmsStatusProcessor: Finished");
        }
    }
}
