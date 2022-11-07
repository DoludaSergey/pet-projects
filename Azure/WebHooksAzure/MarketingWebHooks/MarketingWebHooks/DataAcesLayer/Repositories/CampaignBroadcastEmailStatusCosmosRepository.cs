using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities;
using MarketingWebHooks.ResiliencePolicy;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.DataAcesLayer.Repositories
{
    public class CampaignBroadcastEmailStatusCosmosRepository :
        BaseCosmosRepositoryWithGetEmaleStatuses<CampaignBroadcastEmailStatus>, ICampaignBroadcastEmailStatusRepository
    {
        public CampaignBroadcastEmailStatusCosmosRepository(ILoggerFactory loggerFactory, ICosmosRetryPolicy retryPolicy,
                                                  CosmosSettings cosmosSettings, CosmosClient cosmosClient)
        {
            _logger = loggerFactory.CreateLogger<CampaignBroadcastEmailStatusCosmosRepository>();
            _retryPolicy = retryPolicy;
            _repositoryName = nameof(CampaignBroadcastEmailStatusCosmosRepository);

            ContainerName = cosmosSettings.CampaignBroadcastEmailStatusContainerName;
            _container = cosmosClient.GetContainer(cosmosSettings.DatabaseName, ContainerName);
        }
    }
}
