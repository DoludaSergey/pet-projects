using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Models.Responses;
using MarketingWebHooks.Providers;

namespace MarketingWebHooks.Services
{
    public class CampaignBroadcastStatisticService : MarketingStatisticServiceBase, ICampaignBroadcastStatisticService
    {
        private readonly ICampaignBroadcastStatisticDetailsWithDatesRepository _campaignBroadcastStatisticRepository;

        public CampaignBroadcastStatisticService(ICampaignBroadcastStatisticDetailsWithDatesRepository campaignBroadcastStatisticRepository)
        {
            _campaignBroadcastStatisticRepository = campaignBroadcastStatisticRepository;
        }

        public async Task<MarketingStatisticResponseModel> GetCampaignBroadcastStatisticAsync(int photographerKey, int eventKey,
            int campaignKey, int broadcastKey, int campaignBroadcastKey)
        {
            string campaignBroadcastStatisticPartialKey = StatisticDetailsRepositoryProvider
                .GetCampaignBroadcastStatisticPartialKey(photographerKey, eventKey, campaignKey,
                    broadcastKey, campaignBroadcastKey);

            var statistic = await _campaignBroadcastStatisticRepository.GetByIdAsnc(campaignBroadcastStatisticPartialKey);

            var response = GetStatisticResponse(statistic);

            return response;
        }
    }
}
