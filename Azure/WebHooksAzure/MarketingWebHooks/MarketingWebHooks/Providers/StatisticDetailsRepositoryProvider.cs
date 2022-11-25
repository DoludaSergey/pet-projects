using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities.Base;
using MarketingWebHooks.Models;

namespace MarketingWebHooks.Providers
{
    public class StatisticDetailsRepositoryProvider : IStatisticDetailsRepositoryProvider
    {
        private readonly ICampaignStatisticDetailsRepository _campaignStatisticRepository;
        private readonly IEventMarketingStatisticDetailsRepository _eventStatisticRepository;
        private readonly IPhotographerMarketingStatisticDetailsRepository _photographerStatisticRepository;
        private readonly Dictionary<Type, IRepository<StatisticDetails>> _statisticRepositoriesDictionary;

        public StatisticDetailsRepositoryProvider(ICampaignStatisticDetailsRepository campaignStatisticRepository,
            IEventMarketingStatisticDetailsRepository eventStatisticRepository,
            IPhotographerMarketingStatisticDetailsRepository photographerStatisticRepository)
        {
            _campaignStatisticRepository = campaignStatisticRepository;
            _eventStatisticRepository = eventStatisticRepository;
            _photographerStatisticRepository = photographerStatisticRepository;

            _statisticRepositoriesDictionary = new Dictionary<Type, IRepository<StatisticDetails>>
                {
                    { typeof(ICampaignStatisticDetailsRepository), _campaignStatisticRepository },
                    { typeof(IEventMarketingStatisticDetailsRepository), _eventStatisticRepository },
                    { typeof(IPhotographerMarketingStatisticDetailsRepository), _photographerStatisticRepository }
                };
        }

        public Dictionary<Type, IRepository<StatisticDetails>> GetStatisticRepositoriesDictionary()
        {
            return _statisticRepositoriesDictionary;
        }

        public IRepository<StatisticDetails> GetRepository(Type type)
        { 
            return _statisticRepositoriesDictionary[type];
        }

        public Dictionary<Type, string> GetPartialKeysDictionary(MarketingStatisticModel marketingStatisticModel)
        {
            string campaignStatisticPartialKey = GetCampaignStatisticPartialKey(marketingStatisticModel.PhotographerKey,
                marketingStatisticModel.EventKey, marketingStatisticModel.CampaignKey);
            string eventStatisticPartialKey = GetEventStatisticPartialKey(marketingStatisticModel.PhotographerKey, marketingStatisticModel.EventKey);
            string photographerStatisticPartialKey = GetPhotographerStatisticPartialKey(marketingStatisticModel.PhotographerKey);

            Dictionary<Type, string> partialKeysDictionary = new Dictionary<Type, string>
                {
                    { typeof(ICampaignStatisticDetailsRepository), campaignStatisticPartialKey },
                    { typeof(IEventMarketingStatisticDetailsRepository), eventStatisticPartialKey },
                    { typeof(IPhotographerMarketingStatisticDetailsRepository), photographerStatisticPartialKey }
                };

            return partialKeysDictionary;
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
