using MarketingWebHooks.Entities.Base;
using MarketingWebHooks.Helpers;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.DataAcesLayer.Repositories
{
    public abstract class BaseCosmosRepositoryWithGetWebhookStatuses<T> : BaseCosmosRepository<T>,
                                 IBaseCosmosRepositoryWithGetWebhookStatuses<T> where T : class, IEntityBaseWithLock
    {

        public async Task<List<T>> GetItemsToProcess(int countToProcess = 100)
        {
            try
            {
                var deltaTime = DateTime.UtcNow
                    .AddHours(EnvironmentVariableHelper.GetEnvironmentVariableOrDefaulf("LOCK_DATA_TIME_DELTA_IN_HOURS", 10));

                var queryString = $"SELECT TOP {countToProcess} * FROM {ContainerName} c WHERE c.IsLocked = false OR c.LockDate < \"{deltaTime:o}\"";

                List<T> items = new List<T>();

                // Measure the performance (Request Units) of queries
                FeedIterator<T> queryable = _container.GetItemQueryIterator<T>(queryString);
                while (queryable.HasMoreResults)
                {
                    FeedResponse<T> queryResponse = await queryable.ReadNextAsync();

                    _logger.LogInformation($"{_repositoryName}|GetEmailStatuses:Query batch consumed {queryResponse.RequestCharge} request units");

                    items.AddRange(queryResponse.Resource.ToList());
                }

                return items;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning($"{_repositoryName}|GetEmailStatuses: NotFound. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{_repositoryName}|GetEmailStatuses: Failed. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");
            }

            return null;
        }

        public void SetLockForProcessItems(List<T> items)
        {
            var lockDate = DateTime.UtcNow;

            foreach (var item in items)
            {
                item.IsLocked = true;
                item.LockDate = lockDate;
            }
        }
    }
}
