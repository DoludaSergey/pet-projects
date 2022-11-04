using MarketingWebHooks.Entities;

namespace MarketingWebHooks.DataAcesLayer
{
    public interface IRepository<T> where T : IEntity
    {
        Task<T?> GetByIdAsnc(string id);

        Task<T?> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task<T?> RemoveAsync(string id);
    }
}
