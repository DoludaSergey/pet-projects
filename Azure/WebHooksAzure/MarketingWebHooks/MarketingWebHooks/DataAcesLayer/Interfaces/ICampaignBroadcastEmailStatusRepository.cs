using MarketingWebHooks.DataAcesLayer.Repositories;
using MarketingWebHooks.Entities;

namespace MarketingWebHooks.DataAcesLayer.Interfaces
{
    public interface ICampaignBroadcastEmailStatusRepository : IBaseCosmosRepositoryWithGetWebhookStatuses<CampaignBroadcastEmailStatus>,
        IRepository<CampaignBroadcastEmailStatus>, IGetItemsToProcess<CampaignBroadcastEmailStatus>
    {
        
    }
}
