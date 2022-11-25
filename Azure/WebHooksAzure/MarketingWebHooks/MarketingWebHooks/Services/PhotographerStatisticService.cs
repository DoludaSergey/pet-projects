using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Models.Responses;
using MarketingWebHooks.Providers;

namespace MarketingWebHooks.Services
{
    public class PhotographerStatisticService : MarketingStatisticServiceBase, IPhotographerStatisticService
    {
        private readonly IPhotographerMarketingStatisticDetailsRepository _photographerStatisticRepository;

        public PhotographerStatisticService(IPhotographerMarketingStatisticDetailsRepository photographerStatisticRepository)
        {
            _photographerStatisticRepository = photographerStatisticRepository;
        }

        public async Task<MarketingStatisticResponseModel> GetPhotographerStatisticAsync(int photographerKey)
        {
            string photographerStatisticPartialKey = StatisticDetailsRepositoryProvider
                .GetPhotographerStatisticPartialKey(photographerKey);

            var statistic = await _photographerStatisticRepository.GetByIdAsnc(photographerStatisticPartialKey);

            var response = GetStatisticResponse(statistic);

            return response;
        }
    }
}
