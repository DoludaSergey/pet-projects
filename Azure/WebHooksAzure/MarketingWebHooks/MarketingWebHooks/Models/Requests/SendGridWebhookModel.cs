using MarketingWebHooks.Entities;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MarketingWebHooks.Models.Requests
{
    public class SendGridWebhookModel
    {
        public int BroadcastKey { get; set; }

        public int CampaignKey { get; set; }

        public int CampaignBroadcastKey { get; set; }        

        public int PhotographerKey { get; set; }

        public int EventKey { get; set; }

        public Guid? FreeDdNotificationGroupKey { get; set; }

        public bool IsFreeDdNotificationForExpiringEvent { get; set; }

        [JsonProperty("response")]
        [JsonPropertyName("response")]
        public string? Response { get; set; }

        [JsonProperty("sg_message_id")]
        [JsonPropertyName("sg_message_id")]
        public string? MessageId { get; set; }

        [JsonProperty("event")]
        [JsonPropertyName("event")]
        public string? Status { get; set; }

        [JsonProperty("email")]
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonProperty("timestamp")]
        [JsonPropertyName("timestamp")]
        public uint TimeStamp { get; set; }

        public CampaignBroadcast ToCampaignBroadcast()
        {
            var campaignBroadcast = new CampaignBroadcast()
            {
                Id = Guid.NewGuid().ToString(),
                BroadcastKey = BroadcastKey,
                CampaignKey = CampaignKey,
                CampaignBroadcastKey = CampaignBroadcastKey,
                PhotographerKey = PhotographerKey,
                EventKey = EventKey,
                Status = Status,
                MessageId = MessageId
            };

            return campaignBroadcast;
        }

        public CampaignBroadcastBase ToCampaignBroadcastBase()
        {
            var campaignBroadcast = new CampaignBroadcastBase()
            {
                Id = Guid.NewGuid().ToString(),
                CampaignBroadcastKey = CampaignBroadcastKey,
                Status = Status,
                MessageId = MessageId,
                TimeStamp = TimeStamp
            };

            return campaignBroadcast;
        }
    }
}
