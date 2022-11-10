namespace MarketingWebHooks.Entities.FreeDdNotification
{
    public class FreeDdSmsNotificationStatus : TwilioWebhookStatuslBase
    {
        public bool IsForExpiredEvent { get; set; }
    }
}
