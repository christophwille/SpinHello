using Fermyon.Spin.Sdk;
using System.Net;

namespace SpinHello;

public static class Handler
{
    private delegate HttpResponse RequestHandlerDelegate(HttpRequest request);

    private static Dictionary<string, RequestHandlerDelegate> _routes = new Dictionary<string, RequestHandlerDelegate>()
    {
        { Warmup.DefaultWarmupUrl, WarmupHandler },
        { "/hello", HelloHandler }
    };

    [HttpHandler]
    public static HttpResponse HandleHttpRequest(HttpRequest request)
    {
        var requestUrl = request.Url.ToLower();
        var routeFound = _routes.TryGetValue(requestUrl, out var handler);

        if (routeFound && null != handler) return handler(request);

        return new HttpResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "text/plain" },
            },
            BodyAsString = "Requested route not found",
        };
    }

    public static HttpResponse HelloHandler(HttpRequest request)
    {
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

    public static HttpResponse WarmupHandler(HttpRequest request)
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

}
