using Microsoft.Azure.Cosmos;
using System.Net;

namespace MarketingWebHooks.ResiliencePolicy
{
    internal class PolicyStatusCodes
    {
        private static List<HttpStatusCode> retriableStatusCodes = new List<HttpStatusCode>()
        {
            HttpStatusCode.RequestTimeout,
            HttpStatusCode.TooManyRequests,
            HttpStatusCode.RequestEntityTooLarge,
            //HttpStatusCode.NotFound,
        };

        public static bool IsExceptionRetriable(CosmosException ex)
        {
            return retriableStatusCodes.Contains(ex.StatusCode) ||
                retriableStatusCodes.Contains((HttpStatusCode)ex.SubStatusCode);
        }
    }
}
