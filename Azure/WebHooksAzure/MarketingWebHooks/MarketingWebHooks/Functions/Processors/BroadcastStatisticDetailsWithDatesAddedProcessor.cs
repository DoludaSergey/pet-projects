//using MarketingWebHooks.DataAcesLayer.Interfaces;
//using MarketingWebHooks.Entities;
//using MarketingWebHooks.Entities.Base;
//using MarketingWebHooks.Enums;
//using Microsoft.Azure.Functions.Worker;
//using Microsoft.Extensions.Logging;

//namespace MarketingWebHooks.Functions.Processors
//{
//    public class BroadcastStatisticDetailsWithDatesAddedProcessor
//    {
//        private readonly ILogger _logger;
//        private readonly IBroadcastStatisticDetailsWithDatesRepository _broadcastStatisticRepository;

//        public BroadcastStatisticDetailsWithDatesAddedProcessor(ILoggerFactory loggerFactory, IBroadcastStatisticDetailsWithDatesRepository broadcastStatisticRepository)
//        {
//            _logger = loggerFactory.CreateLogger<BroadcastStatisticDetailsWithDatesAddedProcessor>();
//            _broadcastStatisticRepository = broadcastStatisticRepository;
//        }

//        [Function("BroadcastStatisticDetailsWithDatesAddedProcessor")]
//        public async Task Run([CosmosDBTrigger(
//            databaseName: "WebHook",
//            collectionName: "BroadcastStatisticDetailsWithDates",
//            ConnectionStringSetting = "COSMOS_CONNECTION",
//            LeaseCollectionName = "BroadcastStatisticDetailsWithDates-leases",
//            CreateLeaseCollectionIfNotExists = true)] IReadOnlyList<BroadcastStatisticDetailsWithDates> inputItems)
//        {
//            try
//            {
//                if (inputItems != null && inputItems.Count > 0)
//                {
//                    _logger.LogInformation("Documents modified: " + inputItems.Count);
//                    _logger.LogInformation("First document Id: " + inputItems[0].Id);

//                    BroadcastStatisticDetails broadcastDetails = null;

//                    foreach (var item in inputItems)
//                    {
//                        string partialKey = $"{item.PhotographerKey}|{item.EventKey}|{item.CampaignKey}";

//                        // Get by partialKey
//                        broadcastDetails = await _broadcastStatisticRepository.GetByIdAsnc(partialKey);

//                        // If it is null create a new
//                        if (broadcastDetails is null)
//                        {
//                            broadcastDetails = new BroadcastStatisticDetails()
//                            {
//                                Id = partialKey,
//                                PhotographerKey = item.PhotographerKey,
//                                EventKey = item.EventKey,
//                                BroadcastKey = item.BroadcastKey,
//                                CampaignKey = item.CampaignKey,
//                                CampaignBroadcastKey = item.CampaignBroadcastKey
//                            };
//                        }                        

//                        var incomingStatus = SendGridEmailStatusHelper.FromString(item.Status);

//                        switch (incomingStatus)
//                        {
//                            case SendGridEmailStatus.Open:

//                                //set OpenedDataTime only after first open
//                                //if (broadcastDetails.OpenedDateTime == null)
//                                //{
//                                //    broadcastDetails.OpenedDateTime = DateTime.UtcNow;
//                                //}

//                                ++broadcastDetails.Opens;
//                                break;
//                            case SendGridEmailStatus.Click:

//                                //set LinkClickedDateTime only after first click
//                                //if (broadcastDetails.LinkClickedDateTime == null)
//                                //{
//                                //    broadcastDetails.LinkClickedDateTime = DateTime.UtcNow;
//                                //}

//                                ++broadcastDetails.Clicks;
//                                break;
//                            case SendGridEmailStatus.Unsubscribe:
//                                //broadcastDetails.OptOutDateTime = DateTime.UtcNow;
//                                ++broadcastDetails.Unsubscribes;
//                                // TODO Send some message for HHIH
//                                break;
//                            default:
//                                break;
//                        }

//                        // Save to db
//                        await _broadcastStatisticRepository.UpdateAsync(broadcastDetails);
//                    }
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
