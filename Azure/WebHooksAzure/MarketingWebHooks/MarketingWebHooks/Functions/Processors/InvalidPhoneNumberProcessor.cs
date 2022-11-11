using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MarketingWebHooks.Functions.Processors
{
    public class InvalidPhoneNumberProcessor
    {
        private readonly ILogger _logger;
        private readonly IInvalidPhoneNumberRepository _invalidPhonesRepository;

        public InvalidPhoneNumberProcessor(ILoggerFactory loggerFactory, IInvalidPhoneNumberRepository repository)
        {
            _logger = loggerFactory.CreateLogger<InvalidPhoneNumberProcessor>();
            _invalidPhonesRepository = repository;
        }

        [Function("InvalidPhoneNumberProcessor")]
        public async Task Run([ServiceBusTrigger(AzurServiceBusQueueMessageService.SERVER_BUS_QUEUE_INVALID_PHONE_NUMBERS_PROCESS, Connection = "SERVER_BUS_QUEUE_CON_STR")] string myQueueItem)
        {
            _logger.LogInformation($"InvalidPhoneNumberProcessor: Started processing message: {myQueueItem}");

            try
            {
                InvalidPhoneNumber webhookModel = JsonSerializer.Deserialize<InvalidPhoneNumber>(myQueueItem);

                _logger.LogInformation($"InvalidPhoneNumberProcessor: the queue message was Deserialized");

                await _invalidPhonesRepository.AddAsync(webhookModel);

                _logger.LogInformation($"InvalidPhoneNumberProcessor: Finished ");
            }
            catch (Exception ex)
            {
                _logger.LogError($"InvalidPhoneNumberProcessor: Failed. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");

                throw;
            }

            _logger.LogInformation("InvalidPhoneNumberProcessor: Finished");
        }
    }
}
