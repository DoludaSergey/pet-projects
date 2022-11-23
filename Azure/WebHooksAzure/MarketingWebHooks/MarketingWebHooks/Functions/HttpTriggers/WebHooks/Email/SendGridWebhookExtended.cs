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
    public class SendGridWebhookExtended
    {
        private readonly ILogger _logger;
        private readonly IQueueMessageService _queueMessageService;

        public SendGridWebhookExtended(ILoggerFactory loggerFactory, IQueueMessageService queueMessageService)
        {
            _logger = loggerFactory.CreateLogger<SendGridWebhookExtended>();
            _queueMessageService = queueMessageService;
        }

        [Function("SendGridWebhookExtended")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData requestData)
        {
            var executeTime = DateTime.UtcNow;
            
            _logger.LogInformation("SendGridWebhookExtended|Start");

            HttpResponseData response;

            try
            {
                var requestBody = await new StreamReader(requestData.Body).ReadToEndAsync();

                _logger.LogInformation($"SendGridWebhookExtended|requestBody - {requestBody}");

                var request = JsonConvert.DeserializeObject<List<SendGridWebhookModelExtended>>(requestBody);

                //var request = await JsonSerializer.DeserializeAsync<List<SendGridWebhookModelExtended>>(requestData.Body);

                if (request is null)
                {
                    _logger.LogWarning("SendGridWebhookExtended| request is null!");

                    response = requestData.CreateResponse(HttpStatusCode.NoContent);
                    response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

                    response.WriteString("Not Processed");

                    _logger.LogInformation("SendGridWebhookExtended|Finish");

                    return response;
                }

                foreach (var item in request)
                {
                    item.CreationDate = executeTime;

                    //this is Free DD email notification
                    if (item.FreeDdNotificationGroupKey.HasValue)
                    {
                        _logger.LogInformation("SendGridWebhookExtended|SendMessageFreeDdNotificationProcessAsync");

                        // Send a message to FreeDdNotificationQueue
                        await _queueMessageService.SendMessageFreeDdNotificationEmailExtendedProcessAsync(item.ToFreeDdEmailNotificationStatusExtended());
                    }
                    //this is Marketing Funnel email broadcast
                    else if (item.CampaignBroadcastKey > 0)
                    {
                        _logger.LogInformation("SendGridWebhookExtended|Start SendMessageCampaignBroadcastEmailExtendedProcessAsync");

                        // Send a message to CampaignBroadcastQueue
                        await _queueMessageService.SendMessageCampaignBroadcastEmailExtendedProcessAsync(item.ToCampaignBroadcastEmailStatusExtended());

                        _logger.LogInformation("SendGridWebhookExtended|Finish SendMessageCampaignBroadcastEmailExtendedProcessAsync");
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

            _logger.LogInformation("SendGridWebhookExtended|Finish");

            return response;
        }
    }
}
