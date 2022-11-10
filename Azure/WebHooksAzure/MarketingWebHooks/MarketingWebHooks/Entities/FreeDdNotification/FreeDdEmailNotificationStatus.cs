namespace MarketingWebHooks.Entities
{
    public class FreeDdEmailNotificationStatus : SendGridWebhookStatusBase, IEntity
    {
        public Guid? FreeDdNotificationGroupKey { get; set; }

        public bool IsFreeDdNotificationForExpiringEvent { get; set; }
    }
}
