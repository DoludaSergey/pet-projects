namespace MarketingWebHooks.Entities
{
    public sealed class CampaignBroadcastEmailStatusExtended : SendGridWebhookStatusBaseExtended, IEntity
    {
        public int CampaignBroadcastKey { get; set; }
    }
}
