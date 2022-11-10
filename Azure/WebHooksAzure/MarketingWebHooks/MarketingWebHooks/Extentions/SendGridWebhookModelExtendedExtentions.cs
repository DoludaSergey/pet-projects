using MarketingWebHooks.Entities;
using MarketingWebHooks.Models.Requests;

namespace MarketingWebHooks.Extentions
{
    public static class SendGridWebhookModelExtendedExtentions
    {
        public static CampaignBroadcastEmailStatusExtended ToCampaignBroadcastEmailStatusExtended(this SendGridWebhookModelExtended model)
        {
            var campaignBroadcast = new CampaignBroadcastEmailStatusExtended()
            {
                Id = Guid.NewGuid().ToString(),
                CampaignBroadcastKey = model.CampaignBroadcastKey,
                Status = model.Status,
                MessageId = model.MessageId,
                TimeStamp = model.TimeStamp,
                CreationDate = model.CreationDate,
                PhotographerKey = model.PhotographerKey,
                EventKey = model.EventKey,
                CampaignKey = model.CampaignKey,
                BroadcastKey = model.BroadcastKey,
                SentDateTime = model.SentDateTime,
            };

            return campaignBroadcast;
        }

        public static FreeDdEmailNotificationStatusExtended ToFreeDdEmailNotificationStatusExtended(this SendGridWebhookModelExtended model)
        {
            var campaignBroadcast = new FreeDdEmailNotificationStatusExtended()
            {
                Id = Guid.NewGuid().ToString(),
                Status = model.Status,
                MessageId = model.MessageId,
                TimeStamp = model.TimeStamp,
                CreationDate = model.CreationDate,
                FreeDdNotificationGroupKey = model.FreeDdNotificationGroupKey,
                IsFreeDdNotificationForExpiringEvent = model.IsFreeDdNotificationForExpiringEvent,
                PhotographerKey = model.PhotographerKey,
                EventKey = model.EventKey,
                CampaignKey = model.CampaignKey,
                BroadcastKey = model.BroadcastKey,
                SentDateTime = model.SentDateTime,
            };

            return campaignBroadcast;
        }
    }
}
