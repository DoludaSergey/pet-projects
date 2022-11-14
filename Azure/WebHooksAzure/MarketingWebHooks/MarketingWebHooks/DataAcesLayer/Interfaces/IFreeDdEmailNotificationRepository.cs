﻿using MarketingWebHooks.DataAcesLayer.Repositories;
using MarketingWebHooks.Entities;

namespace MarketingWebHooks.DataAcesLayer.Interfaces
{
    public interface IFreeDdEmailNotificationRepository : IBaseCosmosRepositoryWithGetWebhookStatuses<FreeDdEmailNotificationStatus>,
        IRepository<FreeDdEmailNotificationStatus>, IGetItemsToProcess<FreeDdEmailNotificationStatus>
    {

    }
}
