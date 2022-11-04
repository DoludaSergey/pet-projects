using Microsoft.Azure.Cosmos;

namespace MarketingWebHooks.DataAcesLayer
{
    public interface ICosmosContext
    {
        Container Container { get; }
    }
}
