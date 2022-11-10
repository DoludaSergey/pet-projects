namespace MarketingWebHooks.Models.Requests
{
    public class TwilioWebhookModelBase
    {
        public string SmsSid { get; set; }

        public string SmsStatus { get; set; }

        public bool IsLocked { get; set; }

        public DateTime LockDate { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
