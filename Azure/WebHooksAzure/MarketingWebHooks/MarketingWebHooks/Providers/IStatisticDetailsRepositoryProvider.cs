using MarketingWebHooks.DataAcesLayer.Interfaces;
using MarketingWebHooks.Entities.Base;
using MarketingWebHooks.Models;

namespace MarketingWebHooks.Providers
{
    public interface IStatisticDetailsRepositoryProvider
    {
        Dictionary<Type, string> GetPartialKeysDictionary(MarketingStatisticModel marketingStatisticModel);
        IRepository<StatisticDetails> GetRepository(Type type);
        Dictionary<Type, IRepository<StatisticDetails>> GetStatisticRepositoriesDictionary();
    }
}