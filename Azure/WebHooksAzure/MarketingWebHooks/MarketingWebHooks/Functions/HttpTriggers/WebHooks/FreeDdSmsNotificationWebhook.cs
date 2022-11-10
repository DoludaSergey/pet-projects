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
    public class FreeDdSmsNotificationWebhook
    {
        private readonly ILogger _logger;
        private readonly IQueueMessageService _queueMessageService;

        public FreeDdSmsNotificationWebhook(ILoggerFactory loggerFactory, IQueueMessageService queueMessageService)
        {
            _logger = loggerFactory.CreateLogger<FreeDdSmsNotificationWebhook>();
            _queueMessageService = queueMessageService;
        }

        [Function("FreeDdSmsNotificationWebhook")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData requestData)
        {
            var executeTime = DateTime.UtcNow;
            
            _logger.LogInformation("FreeDdSmslNotificationWebhook|Start");

            HttpResponseData response;

            try
            {
                var request = await JsonSerializer.DeserializeAsync<TwilioWebhookModelBase>(requestData.Body);
                
                if (request is null)
                {
                    _logger.LogWarning("FreeDdSmslNotificationWebhook| request is null!");

                    response = requestData.CreateResponse(HttpStatusCode.NoContent);
                    response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

                    response.WriteString("Not Handled");

                    _logger.LogInformation("FreeDdSmslNotificationWebhook|Finish");

                    return response;
                }

                request.CreationDate = executeTime;

                await _queueMessageService.SendMessageFreeDdNotificationSmsProcessAsync(request.ToFreeDdSmsNotificationStatus());
            }
            catch (Exception e)
            {

                _logger.LogError(e.Message);
            }

            response = requestData.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Handled");

            _logger.LogInformation("FreeDdSmslNotificationWebhook|Finish");

            return response;
        }
    }
}
