using MarketingWebHooks.Entities.Base;

namespace MarketingWebHooks.DataAcesLayer.Interfaces
{
    public interface IGetItemsToProcess<T> where T : IEntityBaseWithLock
    {
        Task<List<T>> GetItemsToProcess(int countToProcess = 100);
        void SetLockForProcessItems(List<T> items);
    }
}
