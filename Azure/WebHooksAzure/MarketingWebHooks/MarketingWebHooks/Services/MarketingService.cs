﻿using MarketingWebHooks.BL.MarketingStatusProcessors;
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
            switch (statisticStatus)
            {
                case MarketingStatisticStatus.Processed:
                    _marketingStatusProcessor = new DefaultMarketingStatusProcessor();
                    break;
                case MarketingStatisticStatus.Open:
                    _marketingStatusProcessor = new OpenMarketingStatusProcessor();
                    break;
                case MarketingStatisticStatus.Click:
                    _marketingStatusProcessor = new ClickMarketingStatusProcessor();
                    break;
                case MarketingStatisticStatus.Unsubscribe:
                    _marketingStatusProcessor = new UnsubscribeMarketingStatusProcessor();
                    break;
                default:
                    _marketingStatusProcessor = new DefaultMarketingStatusProcessor();
                    break;
            }
        }

        public async Task StatusProcessAsync(MarketingStatisticModel marketingStatisticModel)
        {
            // TODO Do we need process (save) 3 statuses only or all?            
            if (marketingStatisticModel.Status == MarketingStatisticStatus.Processed)
            {
                return;
            }

            MarketingStatusProcessorInit(marketingStatisticModel.Status);

            string campaignBroadcastStatisticPartialKey = 
                GetCampaignBroadcastStatisticPartialKey(marketingStatisticModel.PhotographerKey,
                marketingStatisticModel.EventKey, marketingStatisticModel.CampaignKey, marketingStatisticModel.BroadcastKey,
                marketingStatisticModel.CampaignBroadcastKey);
            string campaignStatisticPartialKey = GetCampaignStatisticPartialKey(marketingStatisticModel.PhotographerKey,
                marketingStatisticModel.EventKey, marketingStatisticModel.CampaignKey);
            string eventStatisticPartialKey =
                GetEventStatisticPartialKey(marketingStatisticModel.PhotographerKey, marketingStatisticModel.EventKey);
            string photographerStatisticPartialKey = GetPhotographerStatisticPartialKey(marketingStatisticModel.PhotographerKey);

            BroadcastStatisticDetailsWithDates? campaignBroadcastDetails = await _campaignBroadcastStatisticRepository.GetByIdAsnc(campaignBroadcastStatisticPartialKey);

            // TODO use parallel tasks
            StatisticDetails? campaignDetails = await GetDataFromRepositoryWithNullCheckAsync(
                (BaseCosmosRepository<StatisticDetails>)_campaignStatisticRepository,
                campaignStatisticPartialKey,
                marketingStatisticModel.CreationDate);

            StatisticDetails? eventDetails = await GetDataFromRepositoryWithNullCheckAsync(
                (BaseCosmosRepository<StatisticDetails>)_eventStatisticRepository,
                eventStatisticPartialKey,
                marketingStatisticModel.CreationDate);

            StatisticDetails? photographerDetails = await GetDataFromRepositoryWithNullCheckAsync(
                (BaseCosmosRepository<StatisticDetails>)_photographerStatisticRepository,
                photographerStatisticPartialKey,
                marketingStatisticModel.CreationDate);
            
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

            List<StatisticDetails> statisticDetailsList = new List<StatisticDetails>
            {
                campaignDetails,
                eventDetails,
                photographerDetails
            };

            _marketingStatusProcessor?.Process(campaignBroadcastDetails, statisticDetailsList,
                marketingStatisticModel.CreationDate);

            // Save to db
            await _campaignBroadcastStatisticRepository.UpdateAsync(campaignBroadcastDetails);
            await _campaignStatisticRepository.UpdateAsync(campaignDetails);
            await _eventStatisticRepository.UpdateAsync(eventDetails);
            await _photographerStatisticRepository.UpdateAsync(photographerDetails);
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

        public async Task<MarketingStatisticResponseModel> GetCampaignBroadcastStatistic(int photographerKey, int eventKey,
            int campaignKey, int broadcastKey, int campaignBroadcastKey)
        {
            string campaignBroadcastStatisticPartialKey = GetCampaignBroadcastStatisticPartialKey(photographerKey,
                        eventKey, campaignKey, broadcastKey, campaignBroadcastKey);

            var statistic = await _campaignBroadcastStatisticRepository.GetByIdAsnc(campaignBroadcastStatisticPartialKey);

            var response = GetStatisticResponse(statistic);

            return response;
        }

        public async Task<MarketingStatisticResponseModel> GetCampaignStatistic(int photographerKey, int eventKey, int campaignKey)
        {
            string campaignStatisticPartialKey = GetCampaignStatisticPartialKey(photographerKey,
                        eventKey, campaignKey);

            var statistic = await _campaignStatisticRepository.GetByIdAsnc(campaignStatisticPartialKey);

            var response = GetStatisticResponse(statistic);

            return response;
        }

        public async Task<MarketingStatisticResponseModel> GetEventStatistic(int photographerKey, int eventKey)
        {
            string eventStatisticPartialKey = GetEventStatisticPartialKey(photographerKey, eventKey);

            var statistic = await _eventStatisticRepository.GetByIdAsnc(eventStatisticPartialKey);

            var response = GetStatisticResponse(statistic);

            return response;
        }

        public async Task<MarketingStatisticResponseModel> GetPhotographerStatistic(int photographerKey)
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
