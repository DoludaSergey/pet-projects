using MarketingWebHooks.Entities;

namespace MarketingWebHooks.DataAcesLayer.Interfaces
{
    public interface IGetEmailStatuses<T> where T : IEntity
    {
        Task<List<T>> GetEmailStatuses(int countToProcess = 100);
    }
}
