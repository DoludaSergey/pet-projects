using MarketingWebHooks.DataAcesLayer.Repositories;
using MarketingWebHooks.Entities;

namespace MarketingWebHooks.DataAcesLayer.Interfaces
{
    public interface ICampaignBroadcastEmailStatusExtendedRepository : IBaseCosmosRepositoryWithGetWebhookStatuses<CampaignBroadcastEmailStatusExtended>,
        IRepository<CampaignBroadcastEmailStatusExtended>, IGetItemsToProcess<CampaignBroadcastEmailStatusExtended>
    {
        
    }
}
