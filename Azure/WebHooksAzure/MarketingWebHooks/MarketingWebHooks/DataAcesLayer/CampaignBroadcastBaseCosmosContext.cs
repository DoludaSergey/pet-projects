using Microsoft.Azure.Cosmos;

namespace MarketingWebHooks.DataAcesLayer
{
    public class CampaignBroadcastBaseCosmosContext : ICampaignBroadcastBaseCosmosContext
    {
        public CampaignBroadcastBaseCosmosContext(CosmosSettings cosmosSettings, CosmosClient cosmosClient) =>
            this.Container = cosmosClient.GetContainer(cosmosSettings.DatabaseName, cosmosSettings.CampaignBroadcastBaseContainerName);

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
