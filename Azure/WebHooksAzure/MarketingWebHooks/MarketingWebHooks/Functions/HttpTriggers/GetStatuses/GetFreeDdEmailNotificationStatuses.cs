using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities;
using MarketingWebHooks.Helpers;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.Functions.HttpTriggers
{
    public class GetFreeDdEmailNotificationStatuses
    {
        private readonly ILogger _logger;
        private readonly IFreeDdEmailNotificationRepository _freeDdElailNotificationRepository;
        private readonly IHttpHelper _httpHelper;

        public GetFreeDdEmailNotificationStatuses(ILoggerFactory loggerFactory, IFreeDdEmailNotificationRepository repository, IHttpHelper httpHelper)
        {
            _logger = loggerFactory.CreateLogger<GetFreeDdEmailNotificationStatuses>();
            _freeDdElailNotificationRepository = repository;
            _httpHelper = httpHelper;
        }

        [Function("GetFreeDdEmailNotificationStatuses")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData requestData)
        {
            _logger.LogInformation("GetFreeDdEmailNotificationStatuses|Start GetItemsWithLockProcessing");

            List<FreeDdEmailNotificationStatus> itemsToProcess = await ItemsLockProcessor
                                                .GetItemsWithLockProcessing(_freeDdElailNotificationRepository, _logger);

            _logger.LogInformation("GetFreeDdEmailNotificationStatuses|Finish GetItemsWithLockProcessing");

            return await _httpHelper.CreateSuccessfulHttpResponseAsync(requestData, itemsToProcess);
        }
    }
}
