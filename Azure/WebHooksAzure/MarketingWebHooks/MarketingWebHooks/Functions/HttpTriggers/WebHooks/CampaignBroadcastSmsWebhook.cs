using Azure.Core;
using MarketingWebHooks.Extentions;
using MarketingWebHooks.Models.Requests;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace MarketingWebHooks.Functions.HttpTriggers
{
    public class CampaignBroadcastSmsWebhook
    {
        private readonly ILogger _logger;
        private readonly IQueueMessageService _queueMessageService;

        public CampaignBroadcastSmsWebhook(ILoggerFactory loggerFactory, IQueueMessageService queueMessageService)
        {
            _logger = loggerFactory.CreateLogger<CampaignBroadcastSmsWebhook>();
            _queueMessageService = queueMessageService;
        }

        [Function("CampaignBroadcastSmsWebhook")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData requestData)
        {
            var executeTime = DateTime.UtcNow;
            
            _logger.LogInformation("CampaignBroadcastSmsWebhook|Start");

            HttpResponseData response;

            try
            {
                var request = await JsonSerializer.DeserializeAsync<TwilioWebhookModelBase>(requestData.Body);
                
                if (request is null)
                {
                    _logger.LogWarning("CampaignBroadcastSmsWebhook| request is null!");

                    response = requestData.CreateResponse(HttpStatusCode.NoContent);
                    response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

                    response.WriteString("Not Handled");

                    _logger.LogInformation("CampaignBroadcastSmsWebhook|Finish");

                    return response;
                }

                request.CreationDate = executeTime;

                await _queueMessageService.SendMessageCampaignBroadcastSmsProcessAsync(request.ToCampaignBroadcastSmsStatus());
            }
            catch (Exception e)
            {

                _logger.LogError(e.Message);
            }

            response = requestData.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Handled");

            _logger.LogInformation("CampaignBroadcastSmsWebhook|Finish");

            return response;
        }
    }
}
