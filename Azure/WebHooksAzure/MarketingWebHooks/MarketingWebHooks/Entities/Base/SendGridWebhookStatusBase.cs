using MarketingWebHooks.Entities.Base;

namespace MarketingWebHooks.Entities
{
    public abstract class SendGridWebhookStatusBase : EntityBaseWithLock
    {
        public string? MessageId { get; set; }

        public string? Status { get; set; }

        public uint TimeStamp { get; set; }        
    }
}
