using System;
using System.Collections.Generic;
using MarketingWebHooks.Entities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MarketingWebHooks.Functions.Processors
{
    public class CampaignBroadcastStatusAddedProcessor
    {
        private readonly ILogger _logger;

        public CampaignBroadcastStatusAddedProcessor(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CampaignBroadcastStatusAddedProcessor>();
        }

        [Function("CampaignBroadcastStatusAddedProcessor")]
        public void Run([CosmosDBTrigger(
            databaseName: "WebHook",
            collectionName: "CampaignBroadcast",
            ConnectionStringSetting = "COSMOS_CONNECTION",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)] IReadOnlyList<CampaignBroadcast> input)
        {
            try
            {
                if (input != null && input.Count > 0)
                {
                    _logger.LogInformation("Documents modified: " + input.Count);
                    _logger.LogInformation("First document Id: " + input[0].Id);
                    //_logger.LogInformation("First document Text: " + input[0].Text);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }          
            
            

            //foreach (var document in input)
            //{
            //    MyClass myClass = JsonConvert.DeserializeObject<MyClass>(document.ToString());
            //}
        }
    }

    public class MyDocument
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public int Number { get; set; }

        public bool Boolean { get; set; }
    }
}
