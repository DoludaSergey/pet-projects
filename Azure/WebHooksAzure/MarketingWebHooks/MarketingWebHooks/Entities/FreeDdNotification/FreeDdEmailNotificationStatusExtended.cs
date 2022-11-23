namespace MarketingWebHooks.Entities
{
    public class FreeDdEmailNotificationStatusExtended : FreeDdEmailNotificationStatus
    {
        public int PhotographerKey { get; set; }

        public int EventKey { get; set; }

        public DateTime? SentDateTime { get; set; }
    }
}
