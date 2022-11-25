using MarketingWebHooks.Models;
using MarketingWebHooks.Models.Responses;

namespace MarketingWebHooks.Services
{
    public interface IMarketingService
    {
        Task<MarketingStatisticResponseModel> GetCampaignBroadcastStatisticAsync(int photographerKey, int eventKey, int campaignKey, int broadcastKey, int campaignBroadcastKey);
        Task<MarketingStatisticResponseModel> GetCampaignStatisticAsync(int photographerKey, int eventKey, int campaignKey);
        Task<MarketingStatisticResponseModel> GetEventStatisticAsync(int photographerKey, int eventKey);
        Task<MarketingStatisticResponseModel> GetPhotographerStatisticAsync(int photographerKey);
        Task StatusProcessAsync(MarketingStatisticModel marketingStatisticModel);
    }
}