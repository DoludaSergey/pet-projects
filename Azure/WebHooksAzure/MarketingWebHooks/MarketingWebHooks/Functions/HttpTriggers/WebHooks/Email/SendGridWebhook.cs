using MarketingWebHooks.Models.Requests;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json;

namespace MarketingWebHooks.Functions.HttpTriggers
{
    public class SendGridWebhook
    {
        private readonly ILogger _logger;
        private readonly IQueueMessageService _queueMessageService;

        public SendGridWebhook(ILoggerFactory loggerFactory, IQueueMessageService queueMessageService)
        {
            _logger = loggerFactory.CreateLogger<SendGridWebhook>();
            _queueMessageService = queueMessageService;
        }

        [Function("SendGridWebhook")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData requestData)
        {
            var executeTime = DateTime.UtcNow;
            
            _logger.LogInformation("SendGridWebhook|Start");

            HttpResponseData response;

            try
            {
                var requestBody = await new StreamReader(requestData.Body).ReadToEndAsync();

                _logger.LogInformation($"SendGridWebhook|requestBody - {requestBody}");

                var request = JsonConvert.DeserializeObject<List<SendGridWebhookModel>>(requestBody);

                //var request = await JsonSerializer.DeserializeAsync<List<SendGridWebhookModel>>(requestBody);
                
                if (request is null || request.Count == 0)
                {
                    _logger.LogWarning("SendGridWebhook| request is null or empty!");

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
                        _logger.LogInformation("SendGridWebhook|SendMessageFreeDdNotificationProcessAsync");

                        // Send a message to FreeDdNotificationQueue
                        await _queueMessageService.SendMessageFreeDdNotificationEmailProcessAsync(item.ToFreeDdNotificationWebhookModel());
                    }
                    //this is Marketing Funnel email broadcast
                    else if (item.CampaignBroadcastKey > 0)
                    {
                        _logger.LogInformation("SendGridWebhook|SendMessageCampaignBroadcastProcessAsync");

                        // Send a message to CampaignBroadcastQueue
                        await _queueMessageService.SendMessageCampaignBroadcastEmailBaseProcessAsync(item.ToCampaignBroadcastEmailStatus());
                    }
                    else 
                    {
                        _logger.LogInformation($"SendGridWebhook|item.CampaignBroadcastKey - {item.CampaignBroadcastKey}, Status - {item.Status}");
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

            _logger.LogInformation("SendGridWebhook|Finish");

            return response;
        }
    }
}
