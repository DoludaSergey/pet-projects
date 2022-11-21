using MarketingWebHooks.Extentions;
using MarketingWebHooks.Models.Requests;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace MarketingWebHooks.Functions.HttpTriggers
{
    public class SendGridWebhookExtebded
    {
        private readonly ILogger _logger;
        private readonly IQueueMessageService _queueMessageService;

        public SendGridWebhookExtebded(ILoggerFactory loggerFactory, IQueueMessageService queueMessageService)
        {
            _logger = loggerFactory.CreateLogger<SendGridWebhookExtebded>();
            _queueMessageService = queueMessageService;
        }

        [Function("SendGridWebhookExtebded")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData requestData)
        {
            var executeTime = DateTime.UtcNow;
            
            _logger.LogInformation("SendGridWebhookExtebded|Start");

            HttpResponseData response;

            try
            {
                var requestBody = await new StreamReader(requestData.Body).ReadToEndAsync();

                _logger.LogInformation($"SendGridWebhookExtebded|requestBody - {requestBody}");

                var request = JsonConvert.DeserializeObject<List<SendGridWebhookModelExtended>>(requestBody);

                //var request = await JsonSerializer.DeserializeAsync<List<SendGridWebhookModelExtended>>(requestData.Body);

                if (request is null)
                {
                    _logger.LogWarning("SendGridWebhookExtebded| request is null!");

                    response = requestData.CreateResponse(HttpStatusCode.NoContent);
                    response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

                    response.WriteString("Not Processed");

                    _logger.LogInformation("SendGridWebhook|Finish");

                    return response;
                }

                foreach (var item in request)
                {
                    item.CreationDate = executeTime;

                    //this is Free DD email notification
                    if (item.FreeDdNotificationGroupKey.HasValue)
                    {
                        _logger.LogInformation("SendGridWebhookExtebded|SendMessageFreeDdNotificationProcessAsync");

                        // Send a message to FreeDdNotificationQueue
                        await _queueMessageService.SendMessageFreeDdNotificationEmailExtendedProcessAsync(item.ToFreeDdEmailNotificationStatusExtended());
                    }
                    //this is Marketing Funnel email broadcast
                    else if (item.CampaignBroadcastKey > 0)
                    {
                        _logger.LogInformation("SendGridWebhookExtebded|Start SendMessageCampaignBroadcastEmailExtendedProcessAsync");

                        // Send a message to CampaignBroadcastQueue
                        await _queueMessageService.SendMessageCampaignBroadcastEmailExtendedProcessAsync(item.ToCampaignBroadcastEmailStatusExtended());

                        _logger.LogInformation("SendGridWebhookExtebded|Finish SendMessageCampaignBroadcastEmailExtendedProcessAsync");
                    }
                }                
            }
            catch (Exception e)
            {

                _logger.LogError(e.Message);
            }

            response = requestData.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Processed");

            _logger.LogInformation("SendGridWebhookExtebded|Finish");

            return response;
        }
    }
}
