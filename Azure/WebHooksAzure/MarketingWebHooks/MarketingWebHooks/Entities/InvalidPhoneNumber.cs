using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MarketingWebHooks.Entities
{
    public class InvalidPhoneNumber : IEntity
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        public string? ResourceSid { get; set; }

        public static InvalidPhoneNumber CreateInvalidPhoneNumber(string resourceSid)
        {
            return new InvalidPhoneNumber()
            {
                Id = Guid.NewGuid().ToString(),
                ResourceSid = resourceSid,
            };
        }
    }
}
