namespace MarketingWebHooks.DataAcesLayer
{
    public class CosmosSettings
    {
        public const string SettingName = "CosmosSettings";

        public string EndPoint { get; set; }

        public string Key { get; set; }

        public string DatabaseName { get; set; }

        public string CampaignBroadcastEmailStatusContainerName { get; set; }

        public string CampaignBroadcastSmsStatusContainerName { get; set; }

        public string CampaignBroadcastBaseContainerName { get; set; }

        public string FreeDdEmailNotificationStatusContainerName { get; set; }

        public string FreeDdSmsNotificationStatusContainerName { get; set; }

        public string InvalidPhoneNumberContainerName { get; set; }
    }
}
