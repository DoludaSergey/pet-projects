using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities.Base;
using MarketingWebHooks.ResiliencePolicy;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.DataAcesLayer.Repositories
{
    public class CampaignBroadcastStatisticDetailsWithDatesCosmosRepository : BaseCosmosRepository<BroadcastStatisticDetailsWithDates>,
        ICampaignBroadcastStatisticDetailsWithDatesRepository
    {
        public CampaignBroadcastStatisticDetailsWithDatesCosmosRepository(ILoggerFactory loggerFactory, ICosmosRetryPolicy retryPolicy,
                                                  CosmosSettings cosmosSettings, CosmosClient cosmosClient)
        {
            _logger = loggerFactory.CreateLogger<CampaignBroadcastStatisticDetailsWithDatesCosmosRepository>();
            _retryPolicy = retryPolicy;
            _repositoryName = nameof(CampaignBroadcastStatisticDetailsWithDatesCosmosRepository);

            ContainerName = cosmosSettings.BroadcastStatisticDetailsWithDatesContainerName;
            _container = cosmosClient.GetContainer(cosmosSettings.DatabaseName, ContainerName);
        }
    }
}
