using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MarketingWebHooks.Entities.Base
{
    public abstract class EntityBase : IEntity
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
