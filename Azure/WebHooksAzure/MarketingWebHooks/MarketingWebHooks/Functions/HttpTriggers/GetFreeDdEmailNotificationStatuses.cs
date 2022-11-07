using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities;
using MarketingWebHooks.Helpers;
using MarketingWebHooks.Models.Responses;
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
            _logger.LogInformation("GetFreeDdEmailNotificationStatuses|Start");

            try
            {
                var emailStatuses = await _freeDdElailNotificationRepository.GetEmailStatuses();

                if (emailStatuses is null)
                {
                    emailStatuses = new List<FreeDdEmailNotificationStatus>();

                    return await _httpHelper.CreateFailedHttpResponseAsync(requestData, emailStatuses);
                }
                var lockDate = DateTime.UtcNow;

                foreach (var status in emailStatuses)
                {
                    status.IsLocked = true;
                    status.LockDate = lockDate;
                }

                await _freeDdElailNotificationRepository.BulkUpdateAsync(emailStatuses);

                _logger.LogInformation("GetFreeDdEmailNotificationStatuses|Finish");

                return await _httpHelper.CreateFailedHttpResponseAsync(requestData, emailStatuses);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                _logger.LogInformation("GetFreeDdEmailNotificationStatuses|Finish");

                var responseModel = new BaseResponseModel(e.Message, false);

                return await _httpHelper.CreateFailedHttpResponseAsync(requestData, responseModel);
            }
        }
    }
}
