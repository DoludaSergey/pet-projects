using MarketingWebHooks.Entities;

namespace MarketingWebHooks.DataAcesLayer.Interfaces
{
    public interface IInvalidPhoneNumberRepository : IRepository<InvalidPhoneNumber>
                                                    , IGetItemsToProcess<InvalidPhoneNumber>
    {

    }
}
