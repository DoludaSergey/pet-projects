using MarketingWebHooks.Entities.Base;

namespace MarketingWebHooks.Entities
{
    public class TwilioWebhookStatuslBase : EntityBaseWithLock
    {
        public string SmsSid { get; set; }

        public string SmsStatus { get; set; }        
    }
}
