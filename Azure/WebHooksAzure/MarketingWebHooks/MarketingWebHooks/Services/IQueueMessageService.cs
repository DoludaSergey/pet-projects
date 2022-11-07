using MarketingWebHooks.Entities;

namespace MarketingWebHooks.Services
{
    public interface IQueueMessageService
    {
        Task SendMessageCampaignBroadcastEmailBaseProcessAsync(CampaignBroadcastEmailStatus data);

        Task SendMessageFreeDdNotificationEmailProcessAsync(FreeDdEmailNotificationStatus data);
    }
}