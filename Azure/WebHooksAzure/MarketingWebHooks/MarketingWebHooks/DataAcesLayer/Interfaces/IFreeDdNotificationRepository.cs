using MarketingWebHooks.Entities;

namespace MarketingWebHooks.DataAcesLayer.Interfaces
{
    public interface IFreeDdNotificationRepository : IRepository<FreeDdEmailNotificationStatus>
                                                    , IGetEmailStatuses<FreeDdEmailNotificationStatus>
    {

    }
}
