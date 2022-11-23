using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities;
using MarketingWebHooks.ResiliencePolicy;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.DataAcesLayer.Repositories
{
    public class FreeDdEmailNotificationCosmosRepository :
        BaseCosmosRepositoryWithGetWebhookStatuses<FreeDdEmailNotificationStatus>, IFreeDdEmailNotificationRepository
    {
        public FreeDdEmailNotificationCosmosRepository(ILoggerFactory loggerFactory, ICosmosRetryPolicy retryPolicy,
                                                  CosmosSettings cosmosSettings, CosmosClient cosmosClient)
        {
            ContainerName = cosmosSettings.FreeDdEmailNotificationStatusContainerName;

            _logger = loggerFactory.CreateLogger<FreeDdEmailNotificationCosmosRepository>();
            _retryPolicy = retryPolicy;
            _repositoryName = nameof(FreeDdEmailNotificationCosmosRepository);            
            _container = cosmosClient.GetContainer(cosmosSettings.DatabaseName, ContainerName);
        }
    }
}
