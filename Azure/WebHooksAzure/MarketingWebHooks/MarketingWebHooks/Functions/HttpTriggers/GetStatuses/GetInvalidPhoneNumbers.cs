using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities.FreeDdNotification;
using MarketingWebHooks.Helpers;
using MarketingWebHooks.Models.Responses;
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
            _logger.LogInformation("GetInvalidPhoneNumbers|Start");

            try
            {
                var emailStatuses = await _invalidPhoneRepository.GetItemsToProcess();

                if (emailStatuses is null)
                {
                    emailStatuses = new ();

                    return await _httpHelper.CreateFailedHttpResponseAsync(requestData, emailStatuses);
                }
                var lockDate = DateTime.UtcNow;

                foreach (var status in emailStatuses)
                {
                    status.IsLocked = true;
                    status.LockDate = lockDate;
                }

                await _invalidPhoneRepository.BulkUpdateAsync(emailStatuses);

                _logger.LogInformation("GetInvalidPhoneNumbers|Finish");

                return await _httpHelper.CreateFailedHttpResponseAsync(requestData, emailStatuses);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                _logger.LogInformation("GetInvalidPhoneNumbers|Finish");

                var responseModel = new BaseResponseModel(e.Message, false);

                return await _httpHelper.CreateFailedHttpResponseAsync(requestData, responseModel);
            }
        }
    }
}
