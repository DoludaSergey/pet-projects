using MarketingWebHooks.Models.Responses;

namespace MarketingWebHooks.Services
{
    public interface ICampaignBroadcastStatisticService
    {
        Task<MarketingStatisticResponseModel> GetCampaignBroadcastStatisticAsync(int photographerKey, int eventKey, int campaignKey, int broadcastKey, int campaignBroadcastKey);
    }
}