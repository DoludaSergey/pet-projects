using MarketingWebHooks.Models;
using MarketingWebHooks.Models.Responses;

namespace MarketingWebHooks.Services
{
    public interface IMarketingService
    {
        Task<MarketingStatisticResponseModel> GetCampaignBroadcastStatistic(int photographerKey, int eventKey, int campaignKey, int broadcastKey, int campaignBroadcastKey);
        Task<MarketingStatisticResponseModel> GetCampaignStatistic(int photographerKey, int eventKey, int campaignKey);
        Task<MarketingStatisticResponseModel> GetEventStatistic(int photographerKey, int eventKey);
        Task<MarketingStatisticResponseModel> GetPhotographerStatistic(int photographerKey);
        Task StatusProcess(MarketingStatisticModel marketingStatisticModel);
    }
}