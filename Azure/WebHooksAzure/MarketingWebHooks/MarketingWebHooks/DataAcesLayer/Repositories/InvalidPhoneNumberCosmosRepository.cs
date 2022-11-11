using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities;
using MarketingWebHooks.ResiliencePolicy;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.DataAcesLayer.Repositories
{
    public class InvalidPhoneNumberCosmosRepository :
        BaseCosmosRepositoryWithGetWebhookStatuses<InvalidPhoneNumber>, IInvalidPhoneNumberRepository
    {
        public InvalidPhoneNumberCosmosRepository(ILoggerFactory loggerFactory, ICosmosRetryPolicy retryPolicy,
                                                  CosmosSettings cosmosSettings, CosmosClient cosmosClient)
        {
            _logger = loggerFactory.CreateLogger<InvalidPhoneNumberCosmosRepository>();
            _retryPolicy = retryPolicy;
            _repositoryName = nameof(InvalidPhoneNumberCosmosRepository);

            ContainerName = cosmosSettings.InvalidPhoneNumberContainerName;
            _container = cosmosClient.GetContainer(cosmosSettings.DatabaseName, ContainerName);
        }
    }
}
