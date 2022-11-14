using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities.FreeDdNotification;
using MarketingWebHooks.Helpers;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.Functions.HttpTriggers
{
    public class GetFreeDdSmsNotificationStatuses
    {
        private readonly ILogger _logger;
        private readonly IFreeDdSmsNotificationRepository _freeDdNotificationRepository;
        private readonly IHttpHelper _httpHelper;

        public GetFreeDdSmsNotificationStatuses(ILoggerFactory loggerFactory, IFreeDdSmsNotificationRepository repository, IHttpHelper httpHelper)
        {
            _logger = loggerFactory.CreateLogger<GetFreeDdSmsNotificationStatuses>();
            _freeDdNotificationRepository = repository;
            _httpHelper = httpHelper;
        }

        [Function("GetFreeDdSmsNotificationStatuses")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData requestData)
        {
            _logger.LogInformation("GetFreeDdSmsNotificationStatuses|Start GetItemsWithLockProcessing");

            List<FreeDdSmsNotificationStatus>? itemsToProcess = await ItemsLockProcessor
                                                .GetItemsWithLockProcessing(_freeDdNotificationRepository, _logger);

            _logger.LogInformation("GetFreeDdSmsNotificationStatuses|Finish GetItemsWithLockProcessing");

            if (itemsToProcess is null)
            {
                _logger.LogInformation("GetFreeDdSmsNotificationStatuses|itemsToProcess is null");

                itemsToProcess = new();

                return await _httpHelper.CreateFailedHttpResponseAsync(requestData, itemsToProcess);
            }

            return await _httpHelper.CreateSuccessfulHttpResponseAsync(requestData, itemsToProcess);
        }
    }
}
