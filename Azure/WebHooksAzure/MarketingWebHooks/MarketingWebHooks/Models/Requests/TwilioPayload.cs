using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MarketingWebHooks.Models.Requests
{
    public class TwilioPayload
    {
        [JsonProperty("resource_sid")]
        [JsonPropertyName("resource_sid")]
        public string ResourceSid { get; set; }
        [JsonProperty("service_sid")]
        [JsonPropertyName("service_sid")]
        public string ServiceSid { get; set; }
        [JsonProperty("error_code")]
        [JsonPropertyName("error_code")]
        public int ErrorCode { get; set; }
    }
}
