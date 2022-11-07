using MarketingWebHooks.Entities;

namespace MarketingWebHooks.DataAcesLayer.Interfaces
{
    public interface ICampaignBroadcastBaseRepository : IRepository<CampaignBroadcastEmailStatus>
    {
        Task BulkUpdateAsync(List<CampaignBroadcastEmailStatus> items);
        Task<List<CampaignBroadcastEmailStatus>> GetEmailStatuses(int countToProcess = 100);
    }
}
