using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities;
using MarketingWebHooks.ResiliencePolicy;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.DataAcesLayer.Repositories
{
    public class FreeDdEmailNotificationCosmosRepository :
        BaseCosmosRepositoryWithGetEmaleStatuses<FreeDdEmailNotificationStatus>, IFreeDdNotificationRepository
    {
        public FreeDdEmailNotificationCosmosRepository(ILoggerFactory loggerFactory, ICosmosRetryPolicy retryPolicy,
                                                  CosmosSettings cosmosSettings, CosmosClient cosmosClient)
        {
            _logger = loggerFactory.CreateLogger<FreeDdEmailNotificationCosmosRepository>();
            _retryPolicy = retryPolicy;
            _repositoryName = nameof(FreeDdEmailNotificationCosmosRepository);

            ContainerName = cosmosSettings.FreeDdEmailNotificationStatusContainerName;
            _container = cosmosClient.GetContainer(cosmosSettings.DatabaseName, ContainerName);
        }
    }
}
