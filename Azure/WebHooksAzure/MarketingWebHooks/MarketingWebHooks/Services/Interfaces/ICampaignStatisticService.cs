using MarketingWebHooks.Models.Responses;

namespace MarketingWebHooks.Services
{
    public interface ICampaignStatisticService
    {
        Task<MarketingStatisticResponseModel> GetCampaignStatisticAsync(int photographerKey, int eventKey, int campaignKey);
    }
}