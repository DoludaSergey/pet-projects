using MarketingWebHooks.DataAcesLayer.Repositories;
using MarketingWebHooks.Entities.FreeDdNotification;

namespace MarketingWebHooks.DataAcesLayer.Interfaces
{
    public interface IFreeDdSmsNotificationRepository : IBaseCosmosRepositoryWithGetWebhookStatuses<FreeDdSmsNotificationStatus>, 
        IRepository<FreeDdSmsNotificationStatus>, IGetItemsToProcess<FreeDdSmsNotificationStatus>
    {

    }
}
