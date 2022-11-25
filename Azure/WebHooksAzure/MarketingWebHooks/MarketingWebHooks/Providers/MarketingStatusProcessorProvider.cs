using MarketingWebHooks.BL.MarketingStatusProcessors;
using MarketingWebHooks.Enums;

namespace MarketingWebHooks.Providers
{
    public class MarketingStatusProcessorProvider
    {
        public static IMarketingStatusProcessor GetProcessor(MarketingStatisticStatus statisticStatus)
        {
            return statisticStatus switch
            {
                MarketingStatisticStatus.Processed => new DefaultMarketingStatusProcessor(),
                MarketingStatisticStatus.Open => new OpenMarketingStatusProcessor(),
                MarketingStatisticStatus.Click => new ClickMarketingStatusProcessor(),
                MarketingStatisticStatus.Unsubscribe => new UnsubscribeMarketingStatusProcessor(),
                _ => new DefaultMarketingStatusProcessor(),
            };
        }
    }
}
