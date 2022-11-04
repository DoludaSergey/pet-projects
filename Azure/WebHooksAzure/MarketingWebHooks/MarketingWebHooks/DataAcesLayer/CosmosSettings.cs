namespace MarketingWebHooks.DataAcesLayer
{
    public class CosmosSettings
    {
        public const string SettingName = "CosmosSettings";

        public string EndPoint { get; set; }

        public string Key { get; set; }

        public string DatabaseName { get; set; }

        public string CampaignBroadcastContainerName { get; set; }

        public string CampaignBroadcastBaseContainerName { get; internal set; }
    }
}
