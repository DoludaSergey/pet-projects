namespace MarketingWebHooks.Entities.Base
{
    public class BroadcastStatisticDetails : StatisticDetails
    {
        public int PhotographerKey { get; set; }

        public int EventKey { get; set; }

        public int BroadcastKey { get; set; }

        public int CampaignKey { get; set; }

        public int CampaignBroadcastKey { get; set; }
    }
}
