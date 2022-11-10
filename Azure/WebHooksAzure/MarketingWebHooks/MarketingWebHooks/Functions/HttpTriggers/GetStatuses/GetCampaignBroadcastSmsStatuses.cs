using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities.CampaignBroadcast;
using MarketingWebHooks.Helpers;
using MarketingWebHooks.Models.Responses;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.Functions.HttpTriggers
{
    public class GetCampaignBroadcastSmsStatuses
    {
        private readonly ILogger _logger;
        private readonly ICampaignBroadcastSmsStatusRepository _campaignBroadcastRepository;
        private readonly IHttpHelper _httpHelper;

        public GetCampaignBroadcastSmsStatuses(ILoggerFactory loggerFactory, ICampaignBroadcastSmsStatusRepository campaignBroadcastRepository, IHttpHelper httpHelper)
        {
            _logger = loggerFactory.CreateLogger<GetCampaignBroadcastSmsStatuses>();
            _campaignBroadcastRepository = campaignBroadcastRepository;
            _httpHelper = httpHelper;
        }

        [Function("GetCampaignBroadcastSmsStatuses")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData requestData)
        {
            _logger.LogInformation("GetCampaignBroadcastSmsStatuses|Start");

            try
            {
                var emailStatuses = await _campaignBroadcastRepository.GetWebhookStatuses();

                if (emailStatuses is null)
                {
                    emailStatuses = new List<CampaignBroadcastSmsStatus>();

                    return await _httpHelper.CreateFailedHttpResponseAsync(requestData, emailStatuses);
                }
                var lockDate = DateTime.UtcNow;

                foreach (var status in emailStatuses)
                {
                    status.IsLocked = true;
                    status.LockDate = lockDate;
                }

                await _campaignBroadcastRepository.BulkUpdateAsync(emailStatuses);

                _logger.LogInformation("GetCampaignBroadcastSmsStatuses|Finish");

                return await _httpHelper.CreateFailedHttpResponseAsync(requestData, emailStatuses);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                _logger.LogInformation("GetCampaignBroadcastSmsStatuses|Finish");

                var responseModel = new BaseResponseModel(e.Message, false);

                return await _httpHelper.CreateFailedHttpResponseAsync(requestData, responseModel);
            }
        }
    }
}
