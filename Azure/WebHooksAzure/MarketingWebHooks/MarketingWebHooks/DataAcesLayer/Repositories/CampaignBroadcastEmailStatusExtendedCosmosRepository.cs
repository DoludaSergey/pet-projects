using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities;
using MarketingWebHooks.ResiliencePolicy;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.DataAcesLayer.Repositories
{
    public class CampaignBroadcastEmailStatusExtendedCosmosRepository : BaseCosmosRepositoryWithGetWebhookStatuses<CampaignBroadcastEmailStatusExtended>,
        ICampaignBroadcastEmailStatusExtendedRepository
    {
        public CampaignBroadcastEmailStatusExtendedCosmosRepository(ILoggerFactory loggerFactory, ICosmosRetryPolicy retryPolicy,
                                                  CosmosSettings cosmosSettings, CosmosClient cosmosClient)
        {
            _logger = loggerFactory.CreateLogger<CampaignBroadcastEmailStatusExtendedCosmosRepository>();
            _retryPolicy = retryPolicy;
            _repositoryName = nameof(CampaignBroadcastEmailStatusExtendedCosmosRepository);

            ContainerName = cosmosSettings.CampaignBroadcastEmailStatusExtendedContainerName;
            _container = cosmosClient.GetContainer(cosmosSettings.DatabaseName, ContainerName);
        }
    }
}
