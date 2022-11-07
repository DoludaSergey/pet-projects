using Azure.Messaging.ServiceBus;
using MarketingWebHooks.Entities;
using MarketingWebHooks.ResiliencePolicy;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MarketingWebHooks.Services
{
    public sealed class AzurServiceBusQueueMessageService : IQueueMessageService
    {
        private readonly ServiceBusClient client;
        private readonly ILogger _logger;
        private readonly IRetryPolicy _retryPolicy;

        public const string SERVER_BUS_QUEUE_CAMPAIGN_BROADCAST_EMAIL_STATUS_PROCESS = "campaign-broadcast-email-status-process";
        public const string SERVER_BUS_QUEUE_CAMPAIGN_BROADCAST_EMAIL_BASE_STATUS_PROCESS = "campaign-broadcast-email-base-status-process";
        public const string SERVER_BUS_QUEUE_FREE_DD_NOTIFICATION_EMAIL_STATUS_PROCESS = "free-dd-notification-email-status-process";


        public AzurServiceBusQueueMessageService(ILoggerFactory loggerFactory, IRetryPolicy retryPolicy)
        {
            var connString = Environment.GetEnvironmentVariable("SERVER_BUS_QUEUE_CON_STR");

            var options = new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpTcp,
                RetryOptions = new ServiceBusRetryOptions()
                {
                    Mode = ServiceBusRetryMode.Fixed,
                    TryTimeout = TimeSpan.FromMinutes(2),
                    //Delay = TimeSpan.FromSeconds(3),
                    MaxRetries = 3
                }
            };

            client = new ServiceBusClient(connString, options);

            _logger = loggerFactory.CreateLogger<AzurServiceBusQueueMessageService>();
            _retryPolicy = retryPolicy;
        }

        public async Task SendMessageCampaignBroadcastEmailBaseProcessAsync(CampaignBroadcastEmailStatus data)
        {
            await this.SendMessageAsync(data, SERVER_BUS_QUEUE_CAMPAIGN_BROADCAST_EMAIL_BASE_STATUS_PROCESS);
        }

        public async Task SendMessageFreeDdNotificationEmailProcessAsync(FreeDdEmailNotificationStatus data)
        {
            await this.SendMessageAsync(data, SERVER_BUS_QUEUE_FREE_DD_NOTIFICATION_EMAIL_STATUS_PROCESS);
        }

        public async Task SendMessageAsync<T>(T data, string senderName)
        {
            await using var sender = client.CreateSender(senderName);

            var body = JsonSerializer.Serialize(data);
            var message = new ServiceBusMessage(body)
            {
                Subject = senderName // Label
            };

            message.ApplicationProperties.Add("Machine", Environment.MachineName);

            await _retryPolicy.RetryPolicyHandler.ExecuteAsync(async () =>
            {
                _logger.LogInformation($"AzurServiceBusQueueMessageService|SendMessageAsync: Started SendMessageAsync to {senderName}");

                await sender.SendMessageAsync(message);
            });

            await sender.CloseAsync();
        }

        public async Task ScheduleMessageAsync<T>(T data, string senderName)
        {
            await using var sender = client.CreateSender(senderName);

            var body = JsonSerializer.Serialize(data);
            var message = new ServiceBusMessage(body)
            {
                Subject = senderName // Label
            };

            //int scheduleMessageTimeDelayInHours = EnvironmentVariableHelper.GetEnvironmentVariableOrDefaulf("MESSAGE_TIME_DELAY_IN_HOURS", 3 * 24);

            var scheduledTime = DateTimeOffset.UtcNow.AddHours(1);

            message.ApplicationProperties.Add("Machine", Environment.MachineName);

            await _retryPolicy.RetryPolicyHandler.ExecuteAsync(async () =>
            {
                _logger.LogInformation($"AzurServiceBusQueueMessageService|ScheduleMessageAsync: Started SendMessageAsync to {senderName}");

                await sender.ScheduleMessageAsync(message, scheduledTime);
            });

            await sender.CloseAsync();
        }
    }
}
