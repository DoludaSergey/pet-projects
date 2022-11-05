using MarketingWebHooks.Entities;

namespace MarketingWebHooks.DataAcesLayer
{
    public interface ICampaignBroadcastBaseRepository : IRepository<CampaignBroadcastBase>
    {
        Task BulkUpdateAsync(List<CampaignBroadcastBase> items);
        Task<List<CampaignBroadcastBase>> GetEmailStatuses(int countToProcess = 100);
    }
}
