using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities;
using MarketingWebHooks.ResiliencePolicy;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.DataAcesLayer.Repositories
{
    public abstract class BaseCosmosRepository<T> : IRepository<T> where T : class, IEntity
    {
        protected Container _container;
        protected ILogger _logger;
        protected ICosmosRetryPolicy _retryPolicy;
        protected string _repositoryName;

        protected string ContainerName { get; set; }

        public async Task<T?> AddAsync(T entity)
        {
            if (entity == null)
            {
                return null;
            }

            PartitionKey partitionKey = new PartitionKey(entity.Id);

            var response = await _retryPolicy.RetryPolicyHandler.ExecuteAsync(async () =>
            {
                _logger.LogInformation($"{_repositoryName}|AddAsync: Started CreateItemAsync {entity.Id}");

                return await _container.CreateItemAsync(entity, partitionKey);
            });

            _logger.LogInformation($"{_repositoryName}|AddAsync RequestCharge - {response.RequestCharge} for {entity.Id}");

            return entity;
        }

        public async Task<T?> GetByIdAsnc(string id)
        {
            try
            {
                PartitionKey partitionKey = new PartitionKey(id);

                var response = await _retryPolicy.RetryPolicyHandler.ExecuteAsync(async () =>
                {
                    _logger.LogInformation($"{_repositoryName}|GetByIdAsnc: Started ReadItemAsync {id}");

                    return await _container.ReadItemAsync<T>(id, partitionKey);
                });

                return response;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning($"{_repositoryName}|GetByIdAsnc: NotFound. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{_repositoryName}|GetByIdAsnc: Failed. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");
            }

            return null;
        }

        public async Task<T?> RemoveAsync(string id)
        {
            try
            {
                PartitionKey partitionKey = new PartitionKey(id);

                var response = await _retryPolicy.RetryPolicyHandler.ExecuteAsync(async () =>
                {
                    _logger.LogInformation($"{_repositoryName}|RemoveAsync: Started DeleteItemAsync {id}");

                    return await _container.DeleteItemAsync<T>(id, partitionKey);
                });

                _logger.LogInformation($"{_repositoryName}|RemoveAsync RequestCharge - {response.RequestCharge} for {id}");

                return null;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning($"{_repositoryName}|RemoveAsync: NotFound. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{_repositoryName}|RemoveAsync: Failed. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");
            }

            return null;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            PartitionKey partitionKey = new PartitionKey(entity.Id);

            var response = await _retryPolicy.RetryPolicyHandler.ExecuteAsync(async () =>
            {
                _logger.LogInformation($"{_repositoryName}|UpdateAsync: Started UpsertItemAsync {entity.Id}");

                return await _container.UpsertItemAsync(entity, partitionKey);
            });

            _logger.LogInformation($"{_repositoryName}|UpdateAsync RequestCharge - {response.RequestCharge} for {entity.Id}");

            return entity;
        }

        public async Task BulkUpdateAsync(List<T> items)
        {
            var concurrentTasks = new List<Task>();

            foreach (var item in items)
            {
                concurrentTasks.Add(_container.UpsertItemAsync(item, new PartitionKey(item.Id)));
            }

            await Task.WhenAll(concurrentTasks);
        }
    }
}
