using MarketingWebHooks.Entities;

namespace MarketingWebHooks.DataAcesLayer.Interfaces
{
    public interface ICampaignBroadcastEmailStatusRepository : IRepository<CampaignBroadcastEmailStatus>,
        IGetItemsToProcess<CampaignBroadcastEmailStatus>
    {
        
    }
}
