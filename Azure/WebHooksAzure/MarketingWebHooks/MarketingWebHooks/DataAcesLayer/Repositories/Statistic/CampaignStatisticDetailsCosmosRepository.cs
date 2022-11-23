using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities.Base;
using MarketingWebHooks.ResiliencePolicy;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.DataAcesLayer.Repositories
{
    public class CampaignStatisticDetailsCosmosRepository : BaseCosmosRepository<StatisticDetails>,
        ICampaignStatisticDetailsRepository
    {
        public CampaignStatisticDetailsCosmosRepository(ILoggerFactory loggerFactory, ICosmosRetryPolicy retryPolicy,
                                                  CosmosSettings cosmosSettings, CosmosClient cosmosClient)
        {
            ContainerName = cosmosSettings.CampaignStatisticDetailsContainerName;

            _logger = loggerFactory.CreateLogger<CampaignStatisticDetailsCosmosRepository>();
            _retryPolicy = retryPolicy;
            _repositoryName = nameof(CampaignStatisticDetailsCosmosRepository);
            _container = cosmosClient.GetContainer(cosmosSettings.DatabaseName, ContainerName);
        }
    }
}
