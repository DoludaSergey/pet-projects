using MarketingWebHooks.Models.Responses;

namespace MarketingWebHooks.Services
{
    public interface IEventStatisticService
    {
        Task<MarketingStatisticResponseModel> GetEventStatisticAsync(int photographerKey, int eventKey);
    }
}