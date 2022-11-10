using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MarketingWebHooks.Models.Requests
{
    public class TwilioErrorWebhookModel
    {
        [JsonProperty("Payload")]
        [JsonPropertyName("Payload")]
        public TwilioPayload Payload { get; set; }
        public string Level { get; set; }
        public string Timestamp { get; set; }
        public string Sid { get; set; }
        //public string PayloadType { get; set; }
        //public string AccountSid { get; set; }
    }
}
