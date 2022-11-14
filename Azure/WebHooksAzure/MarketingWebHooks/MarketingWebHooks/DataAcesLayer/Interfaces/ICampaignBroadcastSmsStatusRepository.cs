using MarketingWebHooks.DataAcesLayer.Repositories;
using MarketingWebHooks.Entities.CampaignBroadcast;

namespace MarketingWebHooks.DataAcesLayer.Interfaces
{
    public interface ICampaignBroadcastSmsStatusRepository : IBaseCosmosRepositoryWithGetWebhookStatuses<CampaignBroadcastSmsStatus>,
        IRepository<CampaignBroadcastSmsStatus>, IGetItemsToProcess<CampaignBroadcastSmsStatus>
    {
        
    }
}
