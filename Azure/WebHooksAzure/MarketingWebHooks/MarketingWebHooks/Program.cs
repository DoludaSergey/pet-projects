using Azure.Core;
using MarketingWebHooks.DataAcesLayer;
using MarketingWebHooks.Helpers;
using MarketingWebHooks.ResiliencePolicy;
using MarketingWebHooks.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace MarketingWebHooks
{
    public class Program
    {
        private static IConfiguration config;

        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureFunctionsWorkerDefaults(workerApplication =>
                {
                    // Register our custom middleware with the worker
                    //workerApplication.UseNewtonsoftJson();
                })
                .ConfigureAppConfiguration(c =>
                {
#if DEBUG
                    c.AddJsonFile("local.settings.json", false, false);

#endif
                    c.AddEnvironmentVariables();

                    config = c.Build();
                })
                .ConfigureServices(services =>
                {
                    //services.AddAzureAppConfiguration();

                    //services.AddSingleton<IImageResizer, ImageSharpResizer>();
                    //services.AddSingleton<IUploadFileValidator, UploadFileValidator>();
                    //services.AddSingleton<IUploadFileHelper, UploadFileHelper>();
                    services.AddSingleton<IHttpHelper, HttpHelper>();
                    services.AddScoped<IMarketingService, MarketingService>();
                    services.AddSingleton<IQueueMessageService, AzurServiceBusQueueMessageService>();
                    services.AddSingleton<IRetryPolicy, RetryPolicy>();

                    services.AddCosmosRepository(config);
                    //services.AddAzureBlobStorage(config);

                    //services.AddHttpClient<IHHIHHttpClient, HHIHHttpClient>(httpClient =>
                    //{
                    //    httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("hhih_api_baseUrl"));
                    //    httpClient.DefaultRequestHeaders.Add("X-hhih-Token", Environment.GetEnvironmentVariable("hhih_api_token"));
                    //});
                })
                .Build();

            await host.RunAsync();
        }
    }
}
    
