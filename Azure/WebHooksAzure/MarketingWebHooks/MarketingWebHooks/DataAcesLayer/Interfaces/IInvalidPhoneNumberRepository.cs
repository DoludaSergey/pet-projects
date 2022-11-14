using MarketingWebHooks.DataAcesLayer.Repositories;
using MarketingWebHooks.Entities;

namespace MarketingWebHooks.DataAcesLayer.Interfaces
{
    public interface IInvalidPhoneNumberRepository : IBaseCosmosRepositoryWithGetWebhookStatuses<InvalidPhoneNumber>,
        IRepository<InvalidPhoneNumber>, IGetItemsToProcess<InvalidPhoneNumber>
    {

    }
}
