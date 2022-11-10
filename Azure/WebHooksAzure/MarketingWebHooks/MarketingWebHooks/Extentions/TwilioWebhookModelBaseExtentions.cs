using MarketingWebHooks.Entities.CampaignBroadcast;
using MarketingWebHooks.Entities.FreeDdNotification;
using MarketingWebHooks.Models.Requests;

namespace MarketingWebHooks.Extentions
{
    public static class TwilioWebhookModelBaseExtentions
    {
        public static CampaignBroadcastSmsStatus ToCampaignBroadcastSmsStatus(this TwilioWebhookModelBase model)
        {
            var campaignBroadcast = new CampaignBroadcastSmsStatus()
            {
                Id = Guid.NewGuid().ToString(),
                SmsSid = model.SmsSid,
                SmsStatus = model.SmsStatus,
                CreationDate = model.CreationDate
            };

            return campaignBroadcast;
        }

        public static FreeDdSmsNotificationStatus ToFreeDdSmsNotificationStatus(this TwilioWebhookModelBase model, bool isForExpiredEvent = false)
        {
            var campaignBroadcast = new FreeDdSmsNotificationStatus()
            {
                Id = Guid.NewGuid().ToString(),
                SmsSid = model.SmsSid,
                SmsStatus = model.SmsStatus,
                CreationDate = model.CreationDate
            };

            if (isForExpiredEvent)
            {
                campaignBroadcast.IsForExpiredEvent = true;
            }

            return campaignBroadcast;
        }
    }
}
