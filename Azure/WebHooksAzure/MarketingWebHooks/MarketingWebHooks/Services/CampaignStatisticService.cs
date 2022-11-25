using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Models.Responses;
using MarketingWebHooks.Providers;

namespace MarketingWebHooks.Services
{
    public class CampaignStatisticService : MarketingStatisticServiceBase, ICampaignStatisticService
    {
        private readonly ICampaignStatisticDetailsRepository _campaignStatisticRepository;

        public CampaignStatisticService(ICampaignStatisticDetailsRepository campaignStatisticRepository)
        {
            _campaignStatisticRepository = campaignStatisticRepository;
        }

        public async Task<MarketingStatisticResponseModel> GetCampaignStatisticAsync(int photographerKey, int eventKey, int campaignKey)
        {
            string campaignStatisticPartialKey = StatisticDetailsRepositoryProvider
                .GetCampaignStatisticPartialKey(photographerKey, eventKey, campaignKey);

            var statistic = await _campaignStatisticRepository.GetByIdAsnc(campaignStatisticPartialKey);

            var response = GetStatisticResponse(statistic);

            return response;
        }
    }
}
