using MarketingWebHooks.Entities;

namespace MarketingWebHooks.DataAcesLayer.Interfaces
{
    public interface IFreeDdEmailNotificationRepository : IRepository<FreeDdEmailNotificationStatus>
                                                    , IGetItemsToProcess<FreeDdEmailNotificationStatus>
    {

    }
}
