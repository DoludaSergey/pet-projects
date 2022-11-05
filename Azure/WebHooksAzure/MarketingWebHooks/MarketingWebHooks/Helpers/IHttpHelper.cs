using Microsoft.Azure.Functions.Worker.Http;

namespace MarketingWebHooks.Helpers
{
    public interface IHttpHelper
    {
        Task<HttpResponseData> CreateFailedHttpResponseAsync(HttpRequestData req, object data);
        Task<HttpResponseData> CreateSuccessfulHttpResponseAsync(HttpRequestData req, object data);
    }
}