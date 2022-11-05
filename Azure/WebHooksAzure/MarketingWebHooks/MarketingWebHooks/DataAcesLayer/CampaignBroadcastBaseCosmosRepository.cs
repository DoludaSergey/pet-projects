using MarketingWebHooks.Entities;
using MarketingWebHooks.Helpers;
using MarketingWebHooks.ResiliencePolicy;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.DataAcesLayer
{
    public class CampaignBroadcastBaseCosmosRepository : ICampaignBroadcastBaseRepository
    {
        private readonly ICosmosContext _context;
        private readonly ILogger _logger;
        private readonly ICosmosRetryPolicy _retryPolicy;

        public CampaignBroadcastBaseCosmosRepository(ICampaignBroadcastBaseCosmosContext context, ILoggerFactory loggerFactory, ICosmosRetryPolicy retryPolicy)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<CampaignBroadcastBaseCosmosRepository>();
            _retryPolicy = retryPolicy;
        }

        public async Task<CampaignBroadcastBase?> AddAsync(CampaignBroadcastBase entity)
        {
            if (entity == null)
            {
                return null;
            }

            PartitionKey partitionKey = new PartitionKey(entity.Id);

            var response = await _retryPolicy.RetryPolicyHandler.ExecuteAsync(async () =>
            {
                _logger.LogInformation($"CampaignBroadcastBaseCosmosRepository|AddAsync: Started CreateItemAsync {entity.Id}");

                return await _context.Container.CreateItemAsync(entity, partitionKey);
            });

            _logger.LogInformation($"CampaignBroadcastBaseCosmosRepository|AddAsync RequestCharge - {response.RequestCharge} for {entity.Id}");

            return entity;
        }

        public async Task<CampaignBroadcastBase?> GetByIdAsnc(string id)
        {
            try
            {
                PartitionKey partitionKey = new PartitionKey(id);

                var response = await _retryPolicy.RetryPolicyHandler.ExecuteAsync(async () =>
                {
                    _logger.LogInformation($"CampaignBroadcastBaseCosmosRepository|GetByIdAsnc: Started ReadItemAsync {id}");

                    return await _context.Container.ReadItemAsync<CampaignBroadcastBase>(id, partitionKey);
                });

                return response;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning($"CampaignBroadcastBaseCosmosRepository|GetByIdAsnc: NotFound. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"CampaignBroadcastBaseCosmosRepository|GetByIdAsnc: Failed. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");
            }

            return null;
        }

        public async Task<CampaignBroadcastBase?> RemoveAsync(string id)
        {
            try
            {
                PartitionKey partitionKey = new PartitionKey(id);

                var response = await _retryPolicy.RetryPolicyHandler.ExecuteAsync(async () =>
                {
                    _logger.LogInformation($"CampaignBroadcastBaseCosmosRepository|RemoveAsync: Started DeleteItemAsync {id}");

                    return await _context.Container.DeleteItemAsync<CampaignBroadcastBase>(id, partitionKey);
                });

                _logger.LogInformation($"CampaignBroadcastBaseCosmosRepository|RemoveAsync RequestCharge - {response.RequestCharge} for {id}");

                return null;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning($"CampaignBroadcastBaseCosmosRepository|RemoveAsync: NotFound. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"CampaignBroadcastBaseCosmosRepository|RemoveAsync: Failed. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");
            }

            return null;
        }

        public async Task<CampaignBroadcastBase> UpdateAsync(CampaignBroadcastBase entity)
        {
            PartitionKey partitionKey = new PartitionKey(entity.Id);

            var response = await _retryPolicy.RetryPolicyHandler.ExecuteAsync(async () =>
            {
                _logger.LogInformation($"CampaignBroadcastBaseCosmosRepository|UpdateAsync: Started UpsertItemAsync {entity.Id}");

                return await _context.Container.UpsertItemAsync(entity, partitionKey);
            });

            _logger.LogInformation($"CampaignBroadcastBaseCosmosRepository|UpdateAsync RequestCharge - {response.RequestCharge} for {entity.Id}");

            return entity;
        }

        public async Task<List<CampaignBroadcastBase>> GetEmailStatuses(int countToProcess = 100)
        {
            try
            {
                var deltaTime = DateTime.UtcNow
                    .AddHours(EnvironmentVariableHelper.GetEnvironmentVariableOrDefaulf("REBUILD_THUMB_TIME_DELTA_IN_HOURS", 10));
                
                var queryString = $"SELECT TOP {countToProcess} * FROM CampaignBroadcastBase c WHERE c.IsLocked = false AND c.LockDate < \"{deltaTime}\"";

                List<CampaignBroadcastBase> items = new List<CampaignBroadcastBase>();

                // Measure the performance (Request Units) of queries
                FeedIterator<CampaignBroadcastBase> queryable = _context.Container.GetItemQueryIterator<CampaignBroadcastBase>(queryString);
                while (queryable.HasMoreResults)
                {
                    FeedResponse<CampaignBroadcastBase> queryResponse = await queryable.ReadNextAsync();

                    _logger.LogInformation($"CampaignBroadcastBaseCosmosRepository|GetByEventKey:Query batch consumed {queryResponse.RequestCharge} request units");

                    items.AddRange(queryResponse.Resource.ToList());
                }

                return items;

                //var getImageQuery = _context.Container
                //            .GetItemLinqQueryable<Image>(true);

                //return getImageQuery
                //    .Where(x => x.hhihEventKey == eventKey)
                //    .ToList();
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning($"CampaignBroadcastBaseCosmosRepository|GetByEventKey: NotFound. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"CampaignBroadcastBaseCosmosRepository|GetByEventKey: Failed. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");
            }

            return null;
        }

        public async Task BulkUpdateAsync(List<CampaignBroadcastBase> items)
        {
            var concurrentTasks = new List<Task>();

            foreach (var item in items)
            {
                concurrentTasks.Add(_context.Container.UpsertItemAsync(item, new PartitionKey(item.Id)));
            }

            await Task.WhenAll(concurrentTasks);
        }
    }
}
