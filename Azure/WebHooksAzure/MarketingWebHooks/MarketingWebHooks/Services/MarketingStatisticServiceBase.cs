using MarketingWebHooks.Entities.Base;
using MarketingWebHooks.Models.Responses;

namespace MarketingWebHooks.Services
{
    public class MarketingStatisticServiceBase
    {
        protected static MarketingStatisticResponseModel GetStatisticResponse(StatisticDetails? statistic)
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
