using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MarketingWebHooks.Entities
{
    public class TwilioWebhookStatuslBase : IEntity
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        public string SmsSid { get; set; }

        public string SmsStatus { get; set; }

        public bool IsLocked { get; set; }

        public DateTime LockDate { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
