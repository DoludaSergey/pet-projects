using MarketingWebHooks.Entities.Base;

namespace MarketingWebHooks.BL.MarketingStatusProcessors
{
    public class ClickMarketingStatusProcessor : IMarketingStatusProcessor
    {
        public void Process(BroadcastStatisticDetailsWithDates campaignBroadcastDetails,
            List<StatisticDetails> statisticDetails, DateTime createDate)
        {
            if (campaignBroadcastDetails.LinkClickedDateTime == null)
            {
                campaignBroadcastDetails.LinkClickedDateTime = createDate;

                foreach (var item in statisticDetails)
                {
                    ++item.Clicks;
                }
            }

            ++campaignBroadcastDetails.Clicks;
        }
    }
}
