using MarketingWebHooks.Entities;

namespace MarketingWebHooks.DataAcesLayer.Interfaces
{
    public interface IGetWebhookStatuses<T> where T : IEntity
    {
        Task<List<T>> GetWebhookStatuses(int countToProcess = 100);
    }
}
