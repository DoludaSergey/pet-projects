namespace MarketingWebHooks.Entities
{
    public sealed class FreeDdEmailNotificationStatus : WebhookModelBase, IEntity
    {
        public Guid? FreeDdNotificationGroupKey { get; set; }

        public bool IsFreeDdNotificationForExpiringEvent { get; set; }
    }
}
