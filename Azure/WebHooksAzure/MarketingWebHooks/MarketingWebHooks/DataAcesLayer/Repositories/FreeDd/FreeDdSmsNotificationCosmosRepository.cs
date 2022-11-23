using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities.FreeDdNotification;
using MarketingWebHooks.ResiliencePolicy;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.DataAcesLayer.Repositories
{
    public class FreeDdSmsNotificationCosmosRepository :
        BaseCosmosRepositoryWithGetWebhookStatuses<FreeDdSmsNotificationStatus>, IFreeDdSmsNotificationRepository
    {
        public FreeDdSmsNotificationCosmosRepository(ILoggerFactory loggerFactory, ICosmosRetryPolicy retryPolicy,
                                                  CosmosSettings cosmosSettings, CosmosClient cosmosClient)
        {
            _logger = loggerFactory.CreateLogger<FreeDdSmsNotificationCosmosRepository>();
            _retryPolicy = retryPolicy;
            _repositoryName = nameof(FreeDdSmsNotificationCosmosRepository);

            ContainerName = cosmosSettings.FreeDdSmsNotificationStatusContainerName;
            _container = cosmosClient.GetContainer(cosmosSettings.DatabaseName, ContainerName);
        }
    }
}
