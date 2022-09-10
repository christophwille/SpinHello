using Fermyon.Spin.Sdk;
using System.Net;

namespace SpinHello;

public static class Handler
{
    [HttpHandler]
    public static HttpResponse HandleHttpRequest(HttpRequest request)
    {
        if (request.Url == Warmup.DefaultWarmupUrl)
        {
            return new HttpResponse
            {
                StatusCode = HttpStatusCode.OK,
                Headers = new Dictionary<string, string>
            {
                { "Content-Type", "text/plain" },
            },
                BodyAsString = "warmup",
            };
        }
        
        return new HttpResponse
        {
            StatusCode = HttpStatusCode.OK,
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "text/plain" },
            },
            BodyAsString = "Hello from .NET\n",
        };
    }
}
