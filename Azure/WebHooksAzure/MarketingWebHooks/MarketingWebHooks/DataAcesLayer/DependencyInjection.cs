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

            services.AddScoped<ICampaignBroadcastEmailStatusRepository, CampaignBroadcastEmailStatusCosmosRepository>();
            services.AddScoped<ICampaignBroadcastEmailStatusExtendedRepository, CampaignBroadcastEmailStatusExtendedCosmosRepository>();
            services.AddScoped<ICampaignBroadcastSmsStatusRepository, CampaignBroadcastSmsStatusCosmosRepository>();
            services.AddScoped<IFreeDdEmailNotificationRepository, FreeDdEmailNotificationCosmosRepository>();
            services.AddScoped<IFreeDdSmsNotificationRepository, FreeDdSmsNotificationCosmosRepository>();
            services.AddScoped<IInvalidPhoneNumberRepository, InvalidPhoneNumberCosmosRepository>();
            services.AddScoped<ICampaignBroadcastStatisticDetailsWithDatesRepository, CampaignBroadcastStatisticDetailsWithDatesCosmosRepository>();
            services.AddScoped<ICampaignStatisticDetailsRepository, CampaignStatisticDetailsCosmosRepository>();
            services.AddScoped<IEventMarketingStatisticDetailsRepository, EventMarketingStatisticDetailsCosmosRepository>();
            services.AddScoped<IPhotographerMarketingStatisticDetailsRepository, PhotographerMarketingStatisticDetailsCosmosRepository>();

            return services;
        }
    }
}
