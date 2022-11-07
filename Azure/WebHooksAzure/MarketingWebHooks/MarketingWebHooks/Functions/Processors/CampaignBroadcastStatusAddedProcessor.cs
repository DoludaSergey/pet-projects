//using MarketingWebHooks.Entities;
//using Microsoft.Azure.Functions.Worker;
//using Microsoft.Extensions.Logging;

//namespace MarketingWebHooks.Functions.Processors
//{
//    public class CampaignBroadcastStatusAddedProcessor
//    {
//        private readonly ILogger _logger;

//        public CampaignBroadcastStatusAddedProcessor(ILoggerFactory loggerFactory)
//        {
//            _logger = loggerFactory.CreateLogger<CampaignBroadcastStatusAddedProcessor>();
//        }

//        [Function("CampaignBroadcastStatusAddedProcessor")]
//        public void Run([CosmosDBTrigger(
//            databaseName: "WebHook",
//            collectionName: "CampaignBroadcastEmailStatuses",
//            ConnectionStringSetting = "COSMOS_CONNECTION",
//            LeaseCollectionName = "CampaignBroadcastEmailStatuses-leases",
//            CreateLeaseCollectionIfNotExists = true)] IReadOnlyList<FreeDdEmailNotificationStatus> input)
//        {
//            try
//            {
//                if (input != null && input.Count > 0)
//                {
//                    _logger.LogInformation("Documents modified: " + input.Count);
//                    _logger.LogInformation("First document Id: " + input[0].Id);
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex.Message);
//                throw;
//            }
//        }
//    }
//}
