using MarketingWebHooks.BL.MarketingStatusProcessors;
using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.DataAcesLayer.Repositories;
using MarketingWebHooks.Entities.Base;
using MarketingWebHooks.Enums;
using MarketingWebHooks.Models;
using MarketingWebHooks.Models.Responses;

namespace MarketingWebHooks.Services
{
    public class MarketingService : IMarketingService
    {
        private readonly ICampaignBroadcastStatisticDetailsWithDatesRepository _campaignBroadcastStatisticRepository;
        private readonly ICampaignStatisticDetailsRepository _campaignStatisticRepository;
        private readonly IEventMarketingStatisticDetailsRepository _eventStatisticRepository;
        private readonly IPhotographerMarketingStatisticDetailsRepository _photographerStatisticRepository;
        private IMarketingStatusProcessor _marketingStatusProcessor;

        public MarketingService(ICampaignBroadcastStatisticDetailsWithDatesRepository broadcastStatisticRepository,
            ICampaignStatisticDetailsRepository campaignStatisticRepository,
            IEventMarketingStatisticDetailsRepository eventStatisticRepository,
            IPhotographerMarketingStatisticDetailsRepository photographerStatisticRepository)
        {
            _campaignBroadcastStatisticRepository = broadcastStatisticRepository;
            _campaignStatisticRepository = campaignStatisticRepository;
            _eventStatisticRepository = eventStatisticRepository;
            _photographerStatisticRepository = photographerStatisticRepository;
            _marketingStatusProcessor = new DefaultMarketingStatusProcessor();
        }

        public void MarketingStatusProcessorInit(MarketingStatisticStatus statisticStatus)
        {
            _marketingStatusProcessor = statisticStatus switch
            {
                MarketingStatisticStatus.Processed => new DefaultMarketingStatusProcessor(),
                MarketingStatisticStatus.Open => new OpenMarketingStatusProcessor(),
                MarketingStatisticStatus.Click => new ClickMarketingStatusProcessor(),
                MarketingStatisticStatus.Unsubscribe => new UnsubscribeMarketingStatusProcessor(),
                _ => new DefaultMarketingStatusProcessor(),
            };
        }

        public async Task StatusProcessAsync(MarketingStatisticModel marketingStatisticModel)
        {
            // TODO Do we need process (save) 3 statuses only or all?            
            if (marketingStatisticModel.Status == MarketingStatisticStatus.Processed)
            {
                return;
            }

            MarketingStatusProcessorInit(marketingStatisticModel.Status);

            // Get partialKeys
            string campaignBroadcastStatisticPartialKey = 
                GetCampaignBroadcastStatisticPartialKey(marketingStatisticModel.PhotographerKey, marketingStatisticModel.EventKey, 
                marketingStatisticModel.CampaignKey, marketingStatisticModel.BroadcastKey, marketingStatisticModel.CampaignBroadcastKey);
            string campaignStatisticPartialKey = GetCampaignStatisticPartialKey(marketingStatisticModel.PhotographerKey,
                marketingStatisticModel.EventKey, marketingStatisticModel.CampaignKey);
            string eventStatisticPartialKey = GetEventStatisticPartialKey(marketingStatisticModel.PhotographerKey, marketingStatisticModel.EventKey);
            string photographerStatisticPartialKey = GetPhotographerStatisticPartialKey(marketingStatisticModel.PhotographerKey);

            // Get tasks to get statistics
            var taskGetCampaignBroadcastStatistic = _campaignBroadcastStatisticRepository
                .GetByIdAsnc(campaignBroadcastStatisticPartialKey);

            var taskGetCampaignStatistic = GetDataFromRepositoryWithNullCheckAsync(
                (BaseCosmosRepository<StatisticDetails>)_campaignStatisticRepository,
                campaignStatisticPartialKey,
                marketingStatisticModel.CreationDate);

            var taskGetEventStatistic = GetDataFromRepositoryWithNullCheckAsync(
                (BaseCosmosRepository<StatisticDetails>)_eventStatisticRepository,
                eventStatisticPartialKey,
                marketingStatisticModel.CreationDate);

            var taskGetPhotographerStatistic = GetDataFromRepositoryWithNullCheckAsync(
                (BaseCosmosRepository<StatisticDetails>)_photographerStatisticRepository,
                photographerStatisticPartialKey,
                marketingStatisticModel.CreationDate);

            // Run tasks
            await Task.WhenAll(taskGetCampaignBroadcastStatistic, taskGetCampaignStatistic,
                taskGetEventStatistic, taskGetPhotographerStatistic);

            // Get tasks results
            BroadcastStatisticDetailsWithDates? campaignBroadcastDetails = await taskGetCampaignBroadcastStatistic;
            StatisticDetails? campaignDetails = await taskGetCampaignStatistic;
            StatisticDetails? eventDetails = await taskGetEventStatistic;
            StatisticDetails? photographerDetails = await taskGetPhotographerStatistic;

            // If it is null create a new
            if (campaignBroadcastDetails is null)
            {
                campaignBroadcastDetails = new BroadcastStatisticDetailsWithDates()
                {
                    Id = campaignBroadcastStatisticPartialKey,
                    PhotographerKey = marketingStatisticModel.PhotographerKey,
                    EventKey = marketingStatisticModel.EventKey,
                    BroadcastKey = marketingStatisticModel.BroadcastKey,
                    CampaignKey = marketingStatisticModel.CampaignKey,
                    CampaignBroadcastKey = marketingStatisticModel.CampaignBroadcastKey,
                    CreationDate = marketingStatisticModel.CreationDate
                };
            }

            // Get statisticList to process
            List<StatisticDetails> statisticDetailsList = new List<StatisticDetails>
            {
                campaignDetails,
                eventDetails,
                photographerDetails
            };

            _marketingStatusProcessor?.Process(campaignBroadcastDetails, statisticDetailsList,
                marketingStatisticModel.CreationDate);

            // Save to db
            var taskUpdateCampaignBroadcastStatistic = _campaignBroadcastStatisticRepository.UpdateAsync(campaignBroadcastDetails);
            var taskUpdateCampaignStatistic = _campaignStatisticRepository.UpdateAsync(campaignDetails);
            var taskUpdateEventStatistic = _eventStatisticRepository.UpdateAsync(eventDetails);
            var taskUpdatePhotographerStatistic = _photographerStatisticRepository.UpdateAsync(photographerDetails);

            await Task.WhenAll(taskUpdateCampaignBroadcastStatistic, taskUpdateCampaignStatistic,
                taskUpdateEventStatistic, taskUpdatePhotographerStatistic);            
        }

        private async Task<StatisticDetails> GetDataFromRepositoryWithNullCheckAsync(BaseCosmosRepository<StatisticDetails> repository,
            string partialKey, DateTime createDate)
        {
            StatisticDetails? statisticDetails = await repository.GetByIdAsnc(partialKey);

            if (statisticDetails is null)
            {
                statisticDetails = new StatisticDetails()
                {
                    Id = partialKey,
                    CreationDate = createDate
                };
            }

            return statisticDetails;
        }

        public async Task<MarketingStatisticResponseModel> GetCampaignBroadcastStatisticAsync(int photographerKey, int eventKey,
            int campaignKey, int broadcastKey, int campaignBroadcastKey)
        {
            string campaignBroadcastStatisticPartialKey = GetCampaignBroadcastStatisticPartialKey(photographerKey,
                        eventKey, campaignKey, broadcastKey, campaignBroadcastKey);

            var statistic = await _campaignBroadcastStatisticRepository.GetByIdAsnc(campaignBroadcastStatisticPartialKey);

            var response = GetStatisticResponse(statistic);

            return response;
        }

        public async Task<MarketingStatisticResponseModel> GetCampaignStatisticAsync(int photographerKey, int eventKey, int campaignKey)
        {
            string campaignStatisticPartialKey = GetCampaignStatisticPartialKey(photographerKey,
                        eventKey, campaignKey);

            var statistic = await _campaignStatisticRepository.GetByIdAsnc(campaignStatisticPartialKey);

            var response = GetStatisticResponse(statistic);

            return response;
        }

        public async Task<MarketingStatisticResponseModel> GetEventStatisticAsync(int photographerKey, int eventKey)
        {
            string eventStatisticPartialKey = GetEventStatisticPartialKey(photographerKey, eventKey);

            var statistic = await _eventStatisticRepository.GetByIdAsnc(eventStatisticPartialKey);

            var response = GetStatisticResponse(statistic);

            return response;
        }

        public async Task<MarketingStatisticResponseModel> GetPhotographerStatisticAsync(int photographerKey)
        {
            string photographerStatisticPartialKey = GetPhotographerStatisticPartialKey(photographerKey);

            var statistic = await _photographerStatisticRepository.GetByIdAsnc(photographerStatisticPartialKey);

            var response = GetStatisticResponse(statistic);

            return response;
        }

        private static MarketingStatisticResponseModel GetStatisticResponse(StatisticDetails? statistic)
        {
            var responce = new MarketingStatisticResponseModel();

            if (statistic is not null)
            {
                responce.Clicks = statistic.Clicks;
                responce.Opens = statistic.Opens;
                responce.Unsubscribes = statistic.Unsubscribes;
            }

            return responce;
        }

        public static string GetCampaignBroadcastStatisticPartialKey(int photographerKey, int eventKey,
            int campaignKey, int broadcastKey, int campaignBroadcastKey)
        {
            return $"{photographerKey}|{eventKey}|{campaignKey}|{broadcastKey}|{campaignBroadcastKey}";
        }

        public static string GetCampaignStatisticPartialKey(int photographerKey, int eventKey, int campaignKey)
        {
            return $"{photographerKey}|{eventKey}|{campaignKey}";
        }

        public static string GetEventStatisticPartialKey(int photographerKey, int eventKey)
        {
            return $"{photographerKey}|{eventKey}";
        }

        public static string GetPhotographerStatisticPartialKey(int photographerKey)
        {
            return $"{photographerKey}";
        }

    }
}
