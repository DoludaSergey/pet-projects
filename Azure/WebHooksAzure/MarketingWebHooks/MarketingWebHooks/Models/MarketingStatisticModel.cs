using MarketingWebHooks.Enums;

namespace MarketingWebHooks.Models
{
    public class MarketingStatisticModel
    {
        public int PhotographerKey { get; private set; }

        public int EventKey { get; private set; }

        public int CampaignKey { get; private set; }

        public int BroadcastKey { get; private set; }

        public int CampaignBroadcastKey { get; private set; }


        public MarketingStatisticStatus Status { get; private set; }

        public DateTime CreationDate { get; private set; }

        public MarketingStatisticModel(int photographerKey, int eventKey, int campaignKey,
            int broadcastKey, int campaignBroadcastKey, MarketingStatisticStatus status, DateTime creationDate)
        {
            PhotographerKey = photographerKey;
            EventKey = eventKey;
            CampaignKey = campaignKey;
            BroadcastKey = broadcastKey;
            CampaignBroadcastKey = campaignBroadcastKey;
            Status = status;
            CreationDate = creationDate;
        }

        public string CampaignBroadcastStatisticPartialKey => 
            $"{this.PhotographerKey}|{this.EventKey}|{this.CampaignKey}|{this.BroadcastKey}|{this.CampaignBroadcastKey}";

        public string CampaignStatisticPartialKey => $"{this.PhotographerKey}|{this.EventKey}|{this.CampaignKey}";

        public string EventStatisticPartialKey => $"{this.PhotographerKey}|{this.EventKey}";

        public string PhotographerStatisticPartialKey => $"{this.PhotographerKey}";


    }
}
