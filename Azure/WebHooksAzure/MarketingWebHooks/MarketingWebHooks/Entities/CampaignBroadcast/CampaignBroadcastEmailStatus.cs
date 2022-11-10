namespace MarketingWebHooks.Entities
{
    public sealed class CampaignBroadcastEmailStatus : SendGridWebhookStatusBase, IEntity
    {

        public int CampaignBroadcastKey { get; set; }        
    }
}
