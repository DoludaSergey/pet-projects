using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities.FreeDdNotification;
using MarketingWebHooks.Helpers;
using MarketingWebHooks.Models.Responses;
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
            _logger.LogInformation("GetFreeDdSmsNotificationStatuses|Start");

            try
            {
                var emailStatuses = await _freeDdNotificationRepository.GetWebhookStatuses();

                if (emailStatuses is null)
                {
                    emailStatuses = new List<FreeDdSmsNotificationStatus>();

                    return await _httpHelper.CreateFailedHttpResponseAsync(requestData, emailStatuses);
                }
                var lockDate = DateTime.UtcNow;

                foreach (var status in emailStatuses)
                {
                    status.IsLocked = true;
                    status.LockDate = lockDate;
                }

                await _freeDdNotificationRepository.BulkUpdateAsync(emailStatuses);

                _logger.LogInformation("GetFreeDdSmsNotificationStatuses|Finish");

                return await _httpHelper.CreateFailedHttpResponseAsync(requestData, emailStatuses);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                _logger.LogInformation("GetFreeDdSmsNotificationStatuses|Finish");

                var responseModel = new BaseResponseModel(e.Message, false);

                return await _httpHelper.CreateFailedHttpResponseAsync(requestData, responseModel);
            }
        }
    }
}
