using MarketingWebHooks.Entities;
using MarketingWebHooks.Helpers;
using MarketingWebHooks.Models.Requests;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MarketingWebHooks.Functions.HttpTriggers
{
    public class TwilioErrorWebhook
    {
        private readonly ILogger _logger;
        private readonly IHttpHelper _httpHelper;

        public TwilioErrorWebhook(ILoggerFactory loggerFactory, IHttpHelper httpHelper)
        {
            _logger = loggerFactory.CreateLogger<TwilioErrorWebhook>();
            _httpHelper = httpHelper;
        }

        [Function("TwilioErrorWebhook")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData requestData)
        {
            _logger.LogInformation("TwilioErrorWebhook|Start");

            HttpResponseData response;

            try
            {
                var request = await JsonSerializer.DeserializeAsync<TwilioErrorWebhookModel>(requestData.Body);

                if (request is null)
                {
                    _logger.LogWarning("CampaignBroadcastSmsWebhook| request is null!");                    
                    _logger.LogInformation("CampaignBroadcastSmsWebhook|Finish");

                    return await _httpHelper.CreateFailedHttpResponseAsync(requestData, "Not Handled");
                }

                if (request.Level.ToUpper() == "ERROR")
                {
                    if (ShouldProcessErrorCode(request.Payload.ErrorCode))
                    {
                        var invalidPhoneNumber = InvalidPhoneNumber.CreateInvalidPhoneNumber(request.Payload.ResourceSid);


                        // Save to InvalidPhoneNumbers db
                        ;
                    }

                    _logger.LogInformation($"ApiController.TwilioErrorWebhook|ProcessErrorCode from model - {request.Payload.ErrorCode}");
                }
            }
            catch (Exception e)
            {

                _logger.LogError(e.Message);
            }

            _logger.LogInformation("TwilioErrorWebhook|Finish");

            return await _httpHelper.CreateFailedHttpResponseAsync(requestData, "Handled");
        }

        private static bool ShouldProcessErrorCode(int errorCode)
        {
            switch (errorCode)
            {
                //case 30004:
                //case 30005:
                case 30006:
                case 30007:
                    return true;

                default:
                    return false;
            }
        }
    }
}
