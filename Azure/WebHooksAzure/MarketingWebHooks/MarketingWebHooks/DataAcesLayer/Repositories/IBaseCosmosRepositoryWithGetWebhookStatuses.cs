using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities.Base;

namespace MarketingWebHooks.DataAcesLayer.Repositories
{
    public interface IBaseCosmosRepositoryWithGetWebhookStatuses<T> : IRepository<T>, IGetItemsToProcess<T> where T : class, IEntityBaseWithLock
    {
        //Task<List<T>> GetItemsToProcess(int countToProcess = 100);
        //void SetLockForProcessItems(List<T> items);
    }
}