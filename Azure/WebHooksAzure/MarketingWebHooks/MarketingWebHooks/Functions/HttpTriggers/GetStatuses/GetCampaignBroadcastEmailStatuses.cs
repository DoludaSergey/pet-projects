using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities;
using MarketingWebHooks.Helpers;
using MarketingWebHooks.Models.Responses;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.Functions.HttpTriggers
{
    public class GetCampaignBroadcastEmailStatuses
    {
        private readonly ILogger _logger;
        private readonly ICampaignBroadcastEmailStatusRepository _campaignBroadcastRepository;
        private readonly IHttpHelper _httpHelper;

        public GetCampaignBroadcastEmailStatuses(ILoggerFactory loggerFactory, ICampaignBroadcastEmailStatusRepository campaignBroadcastRepository, IHttpHelper httpHelper)
        {
            _logger = loggerFactory.CreateLogger<GetCampaignBroadcastEmailStatuses>();
            _campaignBroadcastRepository = campaignBroadcastRepository;
            _httpHelper = httpHelper;
        }

        [Function("GetCampaignBroadcastEmailStatuses")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData requestData)
        {
            _logger.LogInformation("GetEmailStatuses|Start");

            try
            {
                var emailStatuses = await _campaignBroadcastRepository.GetWebhookStatuses();

                if (emailStatuses is null)
                {
                    emailStatuses = new List<CampaignBroadcastEmailStatus>();

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

                return await _httpHelper.CreateSuccessfulHttpResponseAsync(requestData, emailStatuses);

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
