using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities;
using MarketingWebHooks.Helpers;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.Functions.HttpTriggers
{
    public class GetInvalidPhoneNumbers
    {
        private readonly ILogger _logger;
        private readonly IInvalidPhoneNumberRepository _invalidPhoneRepository;
        private readonly IHttpHelper _httpHelper;

        public GetInvalidPhoneNumbers(ILoggerFactory loggerFactory, IInvalidPhoneNumberRepository repository, IHttpHelper httpHelper)
        {
            _logger = loggerFactory.CreateLogger<GetInvalidPhoneNumbers>();
            _invalidPhoneRepository = repository;
            _httpHelper = httpHelper;
        }

        [Function("GetInvalidPhoneNumbers")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData requestData)
        {
            _logger.LogInformation("GetInvalidPhoneNumbers|Start GetItemsWithLockProcessing");

            List<InvalidPhoneNumber>? itemsToProcess = await ItemsLockProcessor
                                                .GetItemsWithLockProcessing(_invalidPhoneRepository, _logger);

            _logger.LogInformation("GetInvalidPhoneNumbers|Finish GetItemsWithLockProcessing");

            if (itemsToProcess is null)
            {
                _logger.LogInformation("GetInvalidPhoneNumbers|itemsToProcess is null");

                itemsToProcess = new();

                return await _httpHelper.CreateFailedHttpResponseAsync(requestData, itemsToProcess);
            }

            return await _httpHelper.CreateSuccessfulHttpResponseAsync(requestData, itemsToProcess);
        }
    }
}
