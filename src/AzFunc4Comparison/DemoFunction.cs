using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace AzFunc4Comparison
{
    public class DemoFunction
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _log;

        // Changes from default template: DI (no more static Function)
        // DI doesn't work for static Function, and AddLogging only adds ILogger<T>
        public DemoFunction(IHttpClientFactory clientFactory, ILogger<DemoFunction> log)
        {
            _clientFactory = clientFactory;
            _log = log;
        }

        [FunctionName("uuid")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            _log.LogInformation("C# HTTP trigger function processed a request.");

            var httpClient = _clientFactory.CreateClient();
            var msg = new HttpRequestMessage(HttpMethod.Get, "https://httpbin.org/uuid");

            var response = await httpClient.SendAsync(msg);
            var result = await response.Content.ReadAsStringAsync();

            return new OkObjectResult(result);
        }
    }
}
