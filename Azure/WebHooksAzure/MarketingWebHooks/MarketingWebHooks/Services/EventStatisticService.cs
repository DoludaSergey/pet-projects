using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Models.Responses;
using MarketingWebHooks.Providers;

namespace MarketingWebHooks.Services
{
    public class EventStatisticService : MarketingStatisticServiceBase, IEventStatisticService
    {
        private readonly IEventMarketingStatisticDetailsRepository _eventStatisticRepository;

        public EventStatisticService(IEventMarketingStatisticDetailsRepository eventStatisticRepository)
        {
            _eventStatisticRepository = eventStatisticRepository;
        }

        public async Task<MarketingStatisticResponseModel> GetEventStatisticAsync(int photographerKey, int eventKey)
        {
            string eventStatisticPartialKey = StatisticDetailsRepositoryProvider
                .GetEventStatisticPartialKey(photographerKey, eventKey);

            var statistic = await _eventStatisticRepository.GetByIdAsnc(eventStatisticPartialKey);

            var response = GetStatisticResponse(statistic);

            return response;
        }
    }
}
