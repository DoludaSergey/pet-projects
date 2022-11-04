using Microsoft.Azure.Cosmos;

namespace MarketingWebHooks.DataAcesLayer
{
    public class CampaignBroadcastCosmosContext : ICampaignBroadcastCosmosContext
    {
        public CampaignBroadcastCosmosContext(CosmosSettings cosmosSettings, CosmosClient cosmosClient) =>
            this.Container = cosmosClient.GetContainer(cosmosSettings.DatabaseName, cosmosSettings.CampaignBroadcastContainerName);

        public Container Container { get; }

    //    Container container = await createClient.GetDatabase(this.databaseName)
    //.CreateContainerIfNotExistsAsync(new ContainerProperties(this.lwwCollectionName, "/partitionKey")
    //{
    //    ConflictResolutionPolicy = new ConflictResolutionPolicy()
    //    {
    //        Mode = ConflictResolutionMode.LastWriterWins,
    //        ResolutionPath = "/myCustomId",
    //    }
    //});
    }
}
