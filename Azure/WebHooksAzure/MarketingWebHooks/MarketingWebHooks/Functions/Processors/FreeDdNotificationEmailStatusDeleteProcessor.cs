using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.Functions.Processors
{
    public class FreeDdNotificationEmailStatusDeleteProcessor
    {
        private readonly ILogger _logger;
        private readonly IFreeDdEmailNotificationRepository _freeDdNotificationRepository;

        public FreeDdNotificationEmailStatusDeleteProcessor(ILoggerFactory loggerFactory, IFreeDdEmailNotificationRepository repository)
        {
            _logger = loggerFactory.CreateLogger<FreeDdNotificationEmailStatusDeleteProcessor>();
            _freeDdNotificationRepository = repository;
        }

        [Function("FreeDdNotificationEmailStatusDeleteProcessor")]
        public async Task Run([ServiceBusTrigger(AzurServiceBusQueueMessageService.SERVER_BUS_QUEUE_FREE_DD_NOTIFICATION_EMAIL_STATUS_DELETE_PROCESS, Connection = "SERVER_BUS_QUEUE_CON_STR")] string myQueueItem)
        {
            _logger.LogInformation($"FreeDdNotificationEmailStatusDeleteProcessor: Started processing message: {myQueueItem}");

            try
            {
                await _freeDdNotificationRepository.RemoveAsync(myQueueItem);

                _logger.LogInformation($"FreeDdNotificationEmailStatusDeleteProcessor: Finished ");
            }
            catch (Exception ex)
            {
                _logger.LogError($"FreeDdNotificationEmailStatusDeleteProcessor: Failed. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");

                throw;
            }

            _logger.LogInformation("FreeDdNotificationEmailStatusDeleteProcessor: Finished");
        }
    }
}
