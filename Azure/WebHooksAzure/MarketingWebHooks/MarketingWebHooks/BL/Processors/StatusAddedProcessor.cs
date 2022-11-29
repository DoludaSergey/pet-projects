using MarketingWebHooks.BL.MarketingStatusProcessors;
using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities.Base;
using MarketingWebHooks.Enums;
using MarketingWebHooks.Models;
using MarketingWebHooks.Models.Responses;
using MarketingWebHooks.Providers;

namespace MarketingWebHooks.BL.Processors
{
    public class StatusAddedProcessor : IStatusAddedProcessor
    {
        private readonly ICampaignBroadcastStatisticDetailsWithDatesRepository _campaignBroadcastStatisticRepository;
        private readonly IStatisticDetailsRepositoryProvider _statisticDetailsRepositoryProvider;
        private IMarketingStatusProcessor _marketingStatusProcessor;

        public StatusAddedProcessor(ICampaignBroadcastStatisticDetailsWithDatesRepository broadcastStatisticRepository,
            IStatisticDetailsRepositoryProvider statisticDetailsRepositoryProvider)
        {
            _campaignBroadcastStatisticRepository = broadcastStatisticRepository;
            _statisticDetailsRepositoryProvider = statisticDetailsRepositoryProvider;
            _marketingStatusProcessor = new DefaultMarketingStatusProcessor();
        }

        public async Task StatusProcessAsync(MarketingStatisticModel marketingStatisticModel)
        {
            // TODO Do we need process (save) 3 statuses only or all?            
            if (marketingStatisticModel.Status == MarketingStatisticStatus.Processed)
            {
                return;
            }

            MarketingStatusProcessorInit(marketingStatisticModel.Status);

            Dictionary<Type, StatisticDetails> statisticDetailsDictionary = new();
            Dictionary<Type, IRepository<StatisticDetails>> statisticRepositoriesDictionary =
                _statisticDetailsRepositoryProvider.GetStatisticRepositoriesDictionary();

            BroadcastStatisticDetailsWithDates? campaignBroadcastDetails = await GetStatisticDetails(marketingStatisticModel,
                statisticRepositoriesDictionary, statisticDetailsDictionary);

            List<StatisticDetails> statisticDetailsList = statisticDetailsDictionary.Select(x => x.Value).ToList();

            _marketingStatusProcessor?.Process(campaignBroadcastDetails, statisticDetailsList,
                marketingStatisticModel.CreationDate);

            // Save to db
            await SaveStatisticDetails(statisticRepositoriesDictionary, statisticDetailsDictionary, campaignBroadcastDetails);
        }

        /// <summary>
        /// Get BroadcastStatisticDetailsWithDates from db
        /// If it is first call for the campaign broadcast create a new record in db
        /// Also set all statictics details from all statistic containers into statisticRepositoriesDictionary
        /// List of statistic containers provided by StatisticDetailsRepositoryProvider
        /// </summary>
        /// <param name="marketingStatisticModel">Request model</param>
        /// <param name="statisticRepositoriesDictionary">Collection of statistic repositories</param>
        /// <param name="statisticDetailsDictionary">Collection of statistic results from each repository</param>
        /// <returns>BroadcastStatisticDetailsWithDates</returns>
        private async Task<BroadcastStatisticDetailsWithDates> GetStatisticDetails(MarketingStatisticModel marketingStatisticModel,
            Dictionary<Type, IRepository<StatisticDetails>> statisticRepositoriesDictionary,
            Dictionary<Type, StatisticDetails> statisticDetailsDictionary)
        {
            Dictionary<Type, Task<StatisticDetails>> tasksGetByIdDictionary = new();

            string campaignBroadcastStatisticPartialKey = StatisticDetailsRepositoryProvider.
                GetCampaignBroadcastStatisticPartialKey(marketingStatisticModel.PhotographerKey, marketingStatisticModel.EventKey,
                marketingStatisticModel.CampaignKey, marketingStatisticModel.BroadcastKey, marketingStatisticModel.CampaignBroadcastKey);

            var taskGetCampaignBroadcastStatistic = _campaignBroadcastStatisticRepository
                            .GetByIdAsnc(campaignBroadcastStatisticPartialKey);

            List<Task> tasksGetById = new()
            {
                taskGetCampaignBroadcastStatistic
            };

            Dictionary<Type, string> partialKeysDictionary =
                _statisticDetailsRepositoryProvider.GetPartialKeysDictionary(marketingStatisticModel);

            foreach (var item in statisticRepositoriesDictionary)
            {
                var taskGetFromRepository = GetDataFromRepositoryWithNullCheckAsync(statisticRepositoriesDictionary[item.Key],
                                partialKeysDictionary[item.Key], marketingStatisticModel.CreationDate);

                tasksGetById.Add(taskGetFromRepository);
                tasksGetByIdDictionary.Add(item.Key, taskGetFromRepository);
            }

            // Run tasks
            await Task.WhenAll(tasksGetById);

            // Get tasks results
            var campaignBroadcastDetails = await taskGetCampaignBroadcastStatistic;

            foreach (var item in statisticRepositoriesDictionary)
            {
                StatisticDetails? statistic = await tasksGetByIdDictionary[item.Key];
                statisticDetailsDictionary.Add(item.Key, statistic);
            }

            // If it is null create a new
            campaignBroadcastDetails ??= new BroadcastStatisticDetailsWithDates()
            {
                Id = campaignBroadcastStatisticPartialKey,
                PhotographerKey = marketingStatisticModel.PhotographerKey,
                EventKey = marketingStatisticModel.EventKey,
                BroadcastKey = marketingStatisticModel.BroadcastKey,
                CampaignKey = marketingStatisticModel.CampaignKey,
                CampaignBroadcastKey = marketingStatisticModel.CampaignBroadcastKey,
                CreationDate = marketingStatisticModel.CreationDate
            };

            return campaignBroadcastDetails;
        }

        private async Task SaveStatisticDetails(Dictionary<Type, IRepository<StatisticDetails>> statisticRepositoriesDictionary,
            Dictionary<Type, StatisticDetails> statisticDetailsDictionary,
            BroadcastStatisticDetailsWithDates campaignBroadcastDetails)
        {
            var taskUpdateCampaignBroadcastStatistic = _campaignBroadcastStatisticRepository.UpdateAsync(campaignBroadcastDetails);

            List<Task> tasksSaveToDb = new List<Task>
            {
                taskUpdateCampaignBroadcastStatistic
            };

            foreach (var item in statisticDetailsDictionary)
            {
                var repository = statisticRepositoriesDictionary[item.Key];
                var statisticDetails = item.Value;
                var taskUpdate = repository.UpdateAsync(statisticDetails);

                tasksSaveToDb.Add(taskUpdate);
            }

            // Run tasks
            await Task.WhenAll(tasksSaveToDb);
        }

        private async Task<StatisticDetails> GetDataFromRepositoryWithNullCheckAsync(IRepository<StatisticDetails> repository,
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

        private void MarketingStatusProcessorInit(MarketingStatisticStatus statisticStatus)
        {
            _marketingStatusProcessor = MarketingStatusProcessorProvider.GetProcessor(statisticStatus);
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
    }
}
