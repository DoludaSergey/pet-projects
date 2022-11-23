using MarketingWebHooks.Helpers;
using MarketingWebHooks.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.Functions.HttpTriggers
{
    public class GetPhotographerStatistic
    {
        private readonly ILogger _logger;
        private readonly IMarketingService _marketingService;
        private readonly IHttpHelper _httpHelper;

        public GetPhotographerStatistic(ILoggerFactory loggerFactory, IHttpHelper httpHelper, IMarketingService marketingService)
        {
            _logger = loggerFactory.CreateLogger<GetPhotographerStatistic>();
            _httpHelper = httpHelper;
            _marketingService = marketingService;
        }

        [Function("GetPhotographerStatistic")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData requestData)
        {
            _logger.LogInformation("GetPhotographerStatistic|Start");

            int photographerKey = 1;

            var statistic = await _marketingService.GetPhotographerStatistic(photographerKey);

            _logger.LogInformation("GetPhotographerStatistic|Finish");

            return await _httpHelper.CreateSuccessfulHttpResponseAsync(requestData, statistic);
        }
    }
}
