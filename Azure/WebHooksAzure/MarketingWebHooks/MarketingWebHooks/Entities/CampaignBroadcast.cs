using MarketingWebHooks.Enums;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MarketingWebHooks.Entities
{
    public sealed class CampaignBroadcast : IEntity
    {

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonProperty("broadcastkey")]
        [JsonPropertyName("broadcastkey")]
        public int BroadcastKey { get; set; }

        public int CampaignKey { get; set; }

        public int CampaignBroadcastKey { get; set; }

        public int PhotographerKey { get; set; }

        public int EventKey { get; set; }

        public string? MessageId { get; set; }

        public string? Status { get; set; }

        public string? Email { get; set; }

        public DateTime? LastModifyDate { get; set; }

        public DateTime? OpenedDateTime { get; set; }

        public DateTime? LinkClickedDateTime { get; set; }

        public DateTime? SentDateTime { get; set; }

        public DateTime? OptOutDateTime { get; set; }

        public int Opens { get; set; }

        public int Clicks { get; set; }

        public bool IsSent { get; set; }

        public void SetStatus()
        {
            var incomingStatus = SendGridEmailStatusHelper.FromString(Status);

            if (incomingStatus == SendGridEmailStatus.Processed) return;

            this.LastModifyDate = DateTime.UtcNow;

            switch (incomingStatus)
            {
                case SendGridEmailStatus.Open:

                    //set OpenedDataTime only after first open
                    if (this.OpenedDateTime == null)
                    {
                        this.OpenedDateTime = LastModifyDate;
                    }

                    this.Opens++;
                    break;
                case SendGridEmailStatus.Click:

                    //set LinkClickedDateTime only after first click
                    if (this.LinkClickedDateTime == null)
                    {
                        this.LinkClickedDateTime = LastModifyDate;
                    }

                    this.Clicks++;
                    break;
                case SendGridEmailStatus.Unsubscribe:
                    this.OptOutDateTime = LastModifyDate;
                    //this.CampaignContact.OptOutDateTime = DateTime.UtcNow;
                    //this.CampaignContact.CampaignContactStatus = CampaignContactStatus.Unsubscribed;
                    break;
                default:
                    break;
            }
        }
    }
}
