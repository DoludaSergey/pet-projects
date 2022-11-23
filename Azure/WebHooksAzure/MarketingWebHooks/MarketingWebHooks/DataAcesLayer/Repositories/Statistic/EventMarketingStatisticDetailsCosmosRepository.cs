using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities.Base;
using MarketingWebHooks.ResiliencePolicy;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.DataAcesLayer.Repositories
{
    public class EventMarketingStatisticDetailsCosmosRepository : BaseCosmosRepository<StatisticDetails>,
        IEventMarketingStatisticDetailsRepository
    {
        public EventMarketingStatisticDetailsCosmosRepository(ILoggerFactory loggerFactory, ICosmosRetryPolicy retryPolicy,
                                                  CosmosSettings cosmosSettings, CosmosClient cosmosClient)
        {
            ContainerName = cosmosSettings.EventMarketingStatisticDetailsContainerName;

            _logger = loggerFactory.CreateLogger<EventMarketingStatisticDetailsCosmosRepository>();
            _retryPolicy = retryPolicy;
            _repositoryName = nameof(EventMarketingStatisticDetailsCosmosRepository);
            _container = cosmosClient.GetContainer(cosmosSettings.DatabaseName, ContainerName);
        }
    }
}
