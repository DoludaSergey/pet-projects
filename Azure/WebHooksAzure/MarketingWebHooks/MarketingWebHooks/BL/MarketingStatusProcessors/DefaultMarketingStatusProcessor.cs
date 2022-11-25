using MarketingWebHooks.Entities.Base;

namespace MarketingWebHooks.BL.MarketingStatusProcessors
{
    public class DefaultMarketingStatusProcessor : IMarketingStatusProcessor
    {
        public void Process(BroadcastStatisticDetailsWithDates campaignBroadcastDetails,
            List<StatisticDetails> statisticDetails, DateTime createDate)
        {
            // No logic by default
        }
    }
}
