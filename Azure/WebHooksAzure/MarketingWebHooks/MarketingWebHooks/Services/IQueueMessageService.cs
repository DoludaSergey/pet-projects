using MarketingWebHooks.Entities;
using MarketingWebHooks.Models.Requests;

namespace MarketingWebHooks.Services
{
    public interface IQueueMessageService
    {
        Task SendMessageCampaignBroadcastEmailProcessAsync(CampaignBroadcast data);

        Task SendMessageFreeDdNotificationEmailProcessAsync(SendGridWebhookModel data);
    }
}