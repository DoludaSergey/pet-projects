using MarketingWebHooks.Entities;
using MarketingWebHooks.Entities.CampaignBroadcast;
using MarketingWebHooks.Entities.FreeDdNotification;

namespace MarketingWebHooks.Services
{
    public interface IQueueMessageService
    {
        Task SendMessageCampaignBroadcastEmailBaseProcessAsync(CampaignBroadcastEmailStatus data);
        Task SendMessageCampaignBroadcastEmailExtendedProcessAsync(CampaignBroadcastEmailStatusExtended data);
        Task SendMessageCampaignBroadcastSmsProcessAsync(CampaignBroadcastSmsStatus data);
        Task SendMessageFreeDdNotificationEmailExtendedProcessAsync(FreeDdEmailNotificationStatusExtended data);
        Task SendMessageFreeDdNotificationEmailProcessAsync(FreeDdEmailNotificationStatus data);
        Task SendMessageFreeDdNotificationSmsProcessAsync(FreeDdSmsNotificationStatus data);
    }
}