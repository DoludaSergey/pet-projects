﻿using MarketingWebHooks.DataAcesLayer.Repositories;
using MarketingWebHooks.Entities.Base;
using Microsoft.Extensions.Logging;

namespace MarketingWebHooks.Services
{
    public class ItemsLockProcessor
    {
        public static async Task<List<T>?> GetItemsWithLockProcessing<T>(IBaseCosmosRepositoryWithGetWebhookStatuses<T> repository
            , ILogger logger) where T : class, IEntityBaseWithLock
        {
            logger.LogInformation("GetItemsWithLockProcessing|Start");

            try
            {
                logger.LogInformation("GetItemsWithLockProcessing|Start GetItemsToProcess");

                List<T>? itemsToProcess = await repository.GetItemsToProcess();

                logger.LogInformation("GetItemsWithLockProcessing|Finish GetItemsToProcess");

                var count = 0;

                if (itemsToProcess is not null)
                {
                    count = itemsToProcess.Count;

                    logger.LogInformation($"GetItemsWithLockProcessing|Count of itemsToProcess is - {count}");

                    if (count > 0)
                    {
                        repository.SetLockForProcessItems(itemsToProcess);

                        logger.LogInformation("GetItemsWithLockProcessing|Start BulkUpdateAsync");

                        await repository.BulkUpdateAsync(itemsToProcess);

                        logger.LogInformation("GetItemsWithLockProcessing|Finish BulkUpdateAsync");

                        return itemsToProcess;
                    }
                }

                logger.LogInformation("GetItemsWithLockProcessing|itemsToProcess is null");
            }
            catch (Exception e)
            {
                logger.LogError($"GetItemsWithLockProcessing|Error: {e.Message}");
            }

            return null;
        }
    }
}
