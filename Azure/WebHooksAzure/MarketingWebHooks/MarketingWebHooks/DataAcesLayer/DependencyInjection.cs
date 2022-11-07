using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.DataAcesLayer.Repositories;
using MarketingWebHooks.ResiliencePolicy;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarketingWebHooks.DataAcesLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCosmosRepository(
          this IServiceCollection services,
          IConfiguration configuration)
        {
            CosmosSettings cosmosSettings = new CosmosSettings();
            configuration.GetSection(CosmosSettings.SettingName).Bind(cosmosSettings);

            CosmosClientOptions cosmosClientOptions = new CosmosClientOptions
            {
                ConnectionMode = ConnectionMode.Direct,
                ApplicationRegion = "Central US",
                RequestTimeout = TimeSpan.FromMinutes(2),
                IdleTcpConnectionTimeout = TimeSpan.FromMinutes(20),
                EnableContentResponseOnWrite = false,
                AllowBulkExecution = true,
            };

            CosmosClient cosmosClient = new CosmosClient(Environment.GetEnvironmentVariable("COSMOS_END_POINT"),
                       Environment.GetEnvironmentVariable("COSMOS_KEY"), cosmosClientOptions);

            services.AddSingleton<CosmosSettings>(cosmosSettings);
            services.AddSingleton<CosmosClient>(cosmosClient);

            services.AddSingleton<ICosmosRetryPolicy, CosmosRetryPolicy>();

            services.AddScoped<ICampaignBroadcastBaseRepository, CampaignBroadcastEmailStatusCosmosRepository>();
            services.AddScoped<IFreeDdNotificationRepository, FreeDdEmailNotificationCosmosRepository>();

            return services;
        }
    }
}
