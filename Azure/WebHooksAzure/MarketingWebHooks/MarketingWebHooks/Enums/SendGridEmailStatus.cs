using System.ComponentModel;

namespace MarketingWebHooks.Enums
{
    public enum SendGridEmailStatus
    {
        [Description("N/A")]
        NA,
        Pending,
        Failed,
        Sent,
        Accepted,
        Processed,
        Dropped,
        Delivered,
        Deferred,
        Bounce,
        Blocked,
        Open,
        Click,
        SpamReport,
        Unsubscribe,
        GroupUnsubscribe,
        GroupResubscribe
    }

    public static class SendGridEmailStatusHelper
    {
        public static SendGridEmailStatus FromString(string stringStatus)
        {
            switch (stringStatus.ToLower())
            {
                case "pending":
                    return SendGridEmailStatus.Pending;
                case "send":
                case "sent":
                    return SendGridEmailStatus.Sent;
                case "accepted":
                    return SendGridEmailStatus.Accepted;
                case "processed":
                    return SendGridEmailStatus.Processed;
                case "dropped":
                    return SendGridEmailStatus.Dropped;
                case "delivered":
                    return SendGridEmailStatus.Delivered;
                case "deferred":
                    return SendGridEmailStatus.Deferred;
                case "bounce":
                    return SendGridEmailStatus.Bounce;
                case "blocked":
                    return SendGridEmailStatus.Blocked;
                case "open":
                    return SendGridEmailStatus.Open;
                case "click":
                    return SendGridEmailStatus.Click;
                case "spamreport":
                    return SendGridEmailStatus.SpamReport;
                case "unsubscribe":
                    return SendGridEmailStatus.Unsubscribe;
                case "groupunsubscribe":
                    return SendGridEmailStatus.GroupUnsubscribe;
                case "groupresubscribe":
                    return SendGridEmailStatus.GroupResubscribe;
                case "failed":
                    return SendGridEmailStatus.Failed;
                default:
                    return SendGridEmailStatus.NA;
            }
        }
    }
}
