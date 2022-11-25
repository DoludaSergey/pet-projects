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
        private readonly IPhotographerStatisticService _marketingService;
        private readonly IHttpHelper _httpHelper;

        public GetPhotographerStatistic(ILoggerFactory loggerFactory, IHttpHelper httpHelper, IPhotographerStatisticService marketingService)
        {
            _logger = loggerFactory.CreateLogger<GetPhotographerStatistic>();
            _httpHelper = httpHelper;
            _marketingService = marketingService;
        }

        [Function("GetPhotographerStatistic")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData requestData)
        {
            _logger.LogInformation("GetPhotographerStatistic|Start");

            // TODO Get from request
            int photographerKey = 1;

            var statistic = await _marketingService.GetPhotographerStatisticAsync(photographerKey);

            _logger.LogInformation("GetPhotographerStatistic|Finish");

            return await _httpHelper.CreateSuccessfulHttpResponseAsync(requestData, statistic);
        }
    }
}
