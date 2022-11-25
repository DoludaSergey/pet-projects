using MarketingWebHooks.Models;

namespace MarketingWebHooks.BL.Processors
{
    public interface IStatusAddedProcessor
    {
        Task StatusProcessAsync(MarketingStatisticModel marketingStatisticModel);
    }
}