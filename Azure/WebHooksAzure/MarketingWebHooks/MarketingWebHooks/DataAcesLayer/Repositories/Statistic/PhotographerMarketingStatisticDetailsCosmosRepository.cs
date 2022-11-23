using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities.Base;
using MarketingWebHooks.ResiliencePolicy;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.DataAcesLayer.Repositories
{
    public class PhotographerMarketingStatisticDetailsCosmosRepository : BaseCosmosRepository<StatisticDetails>,
        IPhotographerMarketingStatisticDetailsRepository
    {
        public PhotographerMarketingStatisticDetailsCosmosRepository(ILoggerFactory loggerFactory, ICosmosRetryPolicy retryPolicy,
                                                  CosmosSettings cosmosSettings, CosmosClient cosmosClient)
        {
            ContainerName = cosmosSettings.PhotographerMarketingStatisticDetailsContainerName;

            _logger = loggerFactory.CreateLogger<PhotographerMarketingStatisticDetailsCosmosRepository>();
            _retryPolicy = retryPolicy;
            _repositoryName = nameof(PhotographerMarketingStatisticDetailsCosmosRepository);
            _container = cosmosClient.GetContainer(cosmosSettings.DatabaseName, ContainerName);
        }
    }
}
