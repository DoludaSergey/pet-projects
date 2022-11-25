using MarketingWebHooks.Models.Responses;

namespace MarketingWebHooks.Services
{
    public interface IPhotographerStatisticService
    {
        Task<MarketingStatisticResponseModel> GetPhotographerStatisticAsync(int photographerKey);
    }
}