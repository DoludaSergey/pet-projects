namespace MarketingWebHooks.Entities
{
    public class SendGridWebhookStatusBaseExtended : SendGridWebhookStatusBase
    {
        public int PhotographerKey { get; set; }

        public int EventKey { get; set; }

        public int BroadcastKey { get; set; }

        public int CampaignKey { get; set; }

        public DateTime? SentDateTime { get; set; }
    }
}
