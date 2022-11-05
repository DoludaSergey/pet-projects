using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MarketingWebHooks.Entities
{
    public sealed class CampaignBroadcastBase : IEntity
    {

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        public int CampaignBroadcastKey { get; set; }

        public string? MessageId { get; set; }

        public string? Status { get; set; }

        public uint TimeStamp { get; set; }

        public bool IsLocked { get; set; }

        public DateTime LockDate { get; set; }
    }
}
