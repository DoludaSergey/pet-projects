using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities;
using MarketingWebHooks.Entities.CampaignBroadcast;
using MarketingWebHooks.ResiliencePolicy;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.DataAcesLayer.Repositories
{
    public class CampaignBroadcastSmsStatusCosmosRepository :
        BaseCosmosRepositoryWithGetWebhookStatuses<CampaignBroadcastSmsStatus>, ICampaignBroadcastSmsStatusRepository
    {
        public CampaignBroadcastSmsStatusCosmosRepository(ILoggerFactory loggerFactory, ICosmosRetryPolicy retryPolicy,
                                                  CosmosSettings cosmosSettings, CosmosClient cosmosClient)
        {
            _logger = loggerFactory.CreateLogger<CampaignBroadcastSmsStatusCosmosRepository>();
            _retryPolicy = retryPolicy;
            _repositoryName = nameof(CampaignBroadcastSmsStatusCosmosRepository);

            ContainerName = cosmosSettings.CampaignBroadcastSmsStatusContainerName;
            _container = cosmosClient.GetContainer(cosmosSettings.DatabaseName, ContainerName);
        }
    }
}
