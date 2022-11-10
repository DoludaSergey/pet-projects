using MarketingWebHooks.Entities.FreeDdNotification;

namespace MarketingWebHooks.DataAcesLayer.Interfaces
{
    public interface IFreeDdSmsNotificationRepository : IRepository<FreeDdSmsNotificationStatus>
                                                    , IGetWebhookStatuses<FreeDdSmsNotificationStatus>
    {

    }
}
