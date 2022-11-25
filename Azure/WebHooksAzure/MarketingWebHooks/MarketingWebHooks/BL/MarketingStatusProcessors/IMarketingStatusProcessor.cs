using MarketingWebHooks.Entities.Base;

namespace MarketingWebHooks.BL.MarketingStatusProcessors
{
    public interface IMarketingStatusProcessor
    {
        void Process(BroadcastStatisticDetailsWithDates broadcastStatisticDetailsWithDates, List<StatisticDetails> statisticDetails, DateTime createDate);
    }
}
