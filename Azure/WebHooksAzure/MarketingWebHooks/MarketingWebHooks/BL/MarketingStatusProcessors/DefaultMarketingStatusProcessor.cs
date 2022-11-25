using MarketingWebHooks.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
