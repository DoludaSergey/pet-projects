using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using Azure.Core;
using MarketingWebHooks.Enums;
using MarketingWebHooks.Models.Requests;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

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
            _logger.LogInformation("SendGridWebhook|Start");

            HttpResponseData response;

            try
            {
                var request = await JsonSerializer.DeserializeAsync<List<SendGridWebhookModel>>(requestData.Body);
                
                if (request is null)
                {
                    _logger.LogWarning("SendGridWebhook| request is null!");

                    response = requestData.CreateResponse(HttpStatusCode.NoContent);
                    response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

                    response.WriteString("Not Processed");

                    _logger.LogInformation("SendGridWebhook|Finish");

                    return response;
                }

                foreach (var item in request)
                {
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
