using MarketingWebHooks.Entities.CampaignBroadcast;

namespace MarketingWebHooks.DataAcesLayer.Interfaces
{
    public interface ICampaignBroadcastSmsStatusRepository : IRepository<CampaignBroadcastSmsStatus>,
        IGetWebhookStatuses<CampaignBroadcastSmsStatus>
    {
        
    }
}
