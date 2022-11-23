namespace MarketingWebHooks.Entities
{
    public sealed class CampaignBroadcastEmailStatusExtended : SendGridWebhookStatusBaseExtended, IEntity
    {
        public int BroadcastKey { get; set; }

        public int CampaignKey { get; set; }

        public int CampaignBroadcastKey { get; set; }
    }
}
