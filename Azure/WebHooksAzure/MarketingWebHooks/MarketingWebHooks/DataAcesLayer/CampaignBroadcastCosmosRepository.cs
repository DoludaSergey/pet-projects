using MarketingWebHooks.Entities;
using MarketingWebHooks.ResiliencePolicy;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.DataAcesLayer
{
    public class CampaignBroadcastCosmosRepository : ICampaignBroadcastRepository
    {
        private readonly ICosmosContext _context;
        private readonly ILogger _logger;
        private readonly ICosmosRetryPolicy _retryPolicy;

        public CampaignBroadcastCosmosRepository(ICampaignBroadcastCosmosContext context, ILoggerFactory loggerFactory, ICosmosRetryPolicy retryPolicy)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<CampaignBroadcastCosmosRepository>();
            _retryPolicy = retryPolicy;
        }

        public async Task<CampaignBroadcast?> AddAsync(CampaignBroadcast entity)
        {
            if (entity == null)
            {
                return null;
            }

            PartitionKey partitionKey = new PartitionKey(entity.Id);

            var response = await _retryPolicy.RetryPolicyHandler.ExecuteAsync(async () =>
            {
                _logger.LogInformation($"CampaignBroadcastCosmosRepository|AddAsync: Started CreateItemAsync {entity.Id}");

                return await _context.Container.CreateItemAsync(entity, partitionKey);
            });

            _logger.LogInformation($"CampaignBroadcastCosmosRepository|AddAsync RequestCharge - {response.RequestCharge} for {entity.Id}");

            return entity;
        }

        public async Task<CampaignBroadcast?> GetByIdAsnc(string id)
        {
            try
            {
                PartitionKey partitionKey = new PartitionKey(id);

                var response = await _retryPolicy.RetryPolicyHandler.ExecuteAsync(async () =>
                {
                    _logger.LogInformation($"CampaignBroadcastCosmosRepository|GetByIdAsnc: Started ReadItemAsync {id}");

                    return await _context.Container.ReadItemAsync<CampaignBroadcast>(id, partitionKey);
                });

                return response;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning($"CampaignBroadcastCosmosRepository|GetByIdAsnc: NotFound. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"CampaignBroadcastCosmosRepository|GetByIdAsnc: Failed. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");
            }

            return null;
        }

        public async Task<CampaignBroadcast?> RemoveAsync(string id)
        {
            try
            {
                PartitionKey partitionKey = new PartitionKey(id);

                var response = await _retryPolicy.RetryPolicyHandler.ExecuteAsync(async () =>
                {
                    _logger.LogInformation($"CampaignBroadcastCosmosRepository|RemoveAsync: Started DeleteItemAsync {id}");

                    return await _context.Container.DeleteItemAsync<CampaignBroadcast>(id, partitionKey);
                });

                _logger.LogInformation($"CampaignBroadcastCosmosRepository|RemoveAsync RequestCharge - {response.RequestCharge} for {id}");

                return null;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning($"CampaignBroadcastCosmosRepository|RemoveAsync: NotFound. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"CampaignBroadcastCosmosRepository|RemoveAsync: Failed. Exception Message: {ex.Message} : Stack: {ex.StackTrace}");
            }

            return null;
        }

        public async Task<CampaignBroadcast> UpdateAsync(CampaignBroadcast entity)
        {
            PartitionKey partitionKey = new PartitionKey(entity.Id);

            var response = await _retryPolicy.RetryPolicyHandler.ExecuteAsync(async () =>
            {
                _logger.LogInformation($"CampaignBroadcastCosmosRepository|UpdateAsync: Started UpsertItemAsync {entity.Id}");

                return await _context.Container.UpsertItemAsync<CampaignBroadcast>(entity, partitionKey);
            });

            _logger.LogInformation($"CampaignBroadcastCosmosRepository|UpdateAsync RequestCharge - {response.RequestCharge} for {entity.Id}");

            return entity;
        }
    }
}
