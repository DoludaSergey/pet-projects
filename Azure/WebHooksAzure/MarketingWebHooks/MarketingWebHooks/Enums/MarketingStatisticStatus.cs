namespace MarketingWebHooks.Enums
{
    public enum MarketingStatisticStatus
    {
        Processed,
        Open,
        Click,
        Unsubscribe
    }

    public static class MarketingStatisticStatusHelper
    {
        public static MarketingStatisticStatus FromString(string stringStatus)
        {
            switch (stringStatus.ToLower())
            {
                
                case "open":
                    return MarketingStatisticStatus.Open;
                case "click":
                    return MarketingStatisticStatus.Click;
                case "unsubscribe":
                    return MarketingStatisticStatus.Unsubscribe;
                //case "pending":
                //    return SendGridEmailStatus.Pending;
                //case "send":
                //case "sent":
                //    return SendGridEmailStatus.Sent;
                //case "accepted":
                //    return SendGridEmailStatus.Accepted;
                //case "processed":
                //    return MarketingStatisticStatus.Processed;
                //case "dropped":
                //    return SendGridEmailStatus.Dropped;
                //case "delivered":
                //    return SendGridEmailStatus.Delivered;
                //case "deferred":
                //case "bounce":
                //case "blocked":
                //case "spamreport":
                //case "unsubscribe":
                //case "groupunsubscribe":
                //case "groupresubscribe":
                //case "failed":
                //    return MarketingStatisticStatus.Processed;
                default:
                    return MarketingStatisticStatus.Processed;
            }
        }
    }
}
