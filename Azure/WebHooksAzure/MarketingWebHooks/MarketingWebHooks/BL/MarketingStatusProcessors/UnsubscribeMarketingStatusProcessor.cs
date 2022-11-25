using MarketingWebHooks.Entities.Base;

namespace MarketingWebHooks.BL.MarketingStatusProcessors
{
    public class UnsubscribeMarketingStatusProcessor : IMarketingStatusProcessor
    {
        public void Process(BroadcastStatisticDetailsWithDates campaignBroadcastDetails,
            List<StatisticDetails> statisticDetails, DateTime createDate)
        {
            if (campaignBroadcastDetails.OptOutDateTime == null)
            {
                campaignBroadcastDetails.OptOutDateTime = createDate;

                foreach (var item in statisticDetails)
                {
                    ++item.Unsubscribes;
                }
            }

            ++campaignBroadcastDetails.Unsubscribes;
        }
    }
}
