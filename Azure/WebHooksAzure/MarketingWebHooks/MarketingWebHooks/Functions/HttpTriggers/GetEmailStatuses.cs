using MarketingWebHooks.DataAcesLayer;
using MarketingWebHooks.Helpers;
using MarketingWebHooks.Models.Responses;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.Functions.HttpTriggers
{
    public class GetEmailStatuses
    {
        private readonly ILogger _logger;
        private readonly ICampaignBroadcastBaseRepository _campaignBroadcastRepository;
        private readonly IHttpHelper _httpHelper;

        public GetEmailStatuses(ILoggerFactory loggerFactory, ICampaignBroadcastBaseRepository campaignBroadcastRepository, IHttpHelper httpHelper)
        {
            _logger = loggerFactory.CreateLogger<GetEmailStatuses>();
            _campaignBroadcastRepository = campaignBroadcastRepository;
            _httpHelper = httpHelper;
        }

        [Function("GetEmailStatuses")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData requestData)
        {
            _logger.LogInformation("GetEmailStatuses|Start");

            try
            {
                var emailStatuses = await _campaignBroadcastRepository.GetEmailStatuses();

                if (emailStatuses is null)
                {
                    emailStatuses = new List<Entities.CampaignBroadcastBase>();

                    return await _httpHelper.CreateFailedHttpResponseAsync(requestData, emailStatuses);
                }
                var lockDate = DateTime.UtcNow;

                foreach (var status in emailStatuses)
                {
                    status.IsLocked = true;
                    status.LockDate = lockDate;
                }

                await _campaignBroadcastRepository.BulkUpdateAsync(emailStatuses);

                _logger.LogInformation("GetEmailStatuses|Finish");

                return await _httpHelper.CreateFailedHttpResponseAsync(requestData, emailStatuses);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                _logger.LogInformation("GetEmailStatuses|Finish");

                var responseModel = new BaseResponseModel(e.Message, false);

                return await _httpHelper.CreateFailedHttpResponseAsync(requestData, responseModel);
            }
        }
    }
}
