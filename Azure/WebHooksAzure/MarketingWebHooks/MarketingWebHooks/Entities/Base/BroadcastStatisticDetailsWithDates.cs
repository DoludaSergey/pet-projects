namespace MarketingWebHooks.Entities.Base
{
    public class BroadcastStatisticDetailsWithDates : BroadcastStatisticDetails
    {
        public DateTime? OpenedDateTime { get; set; }

        public DateTime? LinkClickedDateTime { get; set; }

        public DateTime? OptOutDateTime { get; set; }
    }
}
