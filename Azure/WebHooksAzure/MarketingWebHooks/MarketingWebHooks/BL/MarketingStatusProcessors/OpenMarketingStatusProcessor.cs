using MarketingWebHooks.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketingWebHooks.BL.MarketingStatusProcessors
{
    public class OpenMarketingStatusProcessor : IMarketingStatusProcessor
    {
        public void Process(BroadcastStatisticDetailsWithDates campaignBroadcastDetails,
            List<StatisticDetails> statisticDetails, DateTime createDate)
        {
            if (campaignBroadcastDetails.OpenedDateTime == null)
            {
                campaignBroadcastDetails.OpenedDateTime = createDate;

                foreach (var item in statisticDetails)
                {
                    ++item.Opens;
                }
            }

            ++campaignBroadcastDetails.Opens;
        }
    }
}
