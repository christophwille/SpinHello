using Fermyon.Spin.Sdk;
using Microsoft.Extensions.Logging;

namespace SpinHello;

public static class Handler
{
    private delegate HttpResponse RequestHandlerDelegate(HttpRequest request);

    private static Dictionary<string, RequestHandlerDelegate> _routes = new Dictionary<string, RequestHandlerDelegate>()
    {
        { Warmup.DefaultWarmupUrl, WarmupHandler },
        { "/hello", HelloHandler },
        { "/uuid", GetUuidHandler }
    };

    static Handler()
    {
        SetDefaultServices();
    }

    public static void SetDefaultServices()
    {
        Logger = new SpinLogger();
        OutboundServices = new DefaultOutboundCommunication();
    }

    // No DI no nothing just for testing it out
    public static IOutboundCommunication OutboundServices;
    public static ILogger Logger;

    [HttpHandler]
    public static HttpResponse HandleHttpRequest(HttpRequest request)
    {
        var requestUrl = request.Url.ToLower();
        var routeFound = _routes.TryGetValue(requestUrl, out var handler);

        if (routeFound && null != handler) return handler(request);

        return new HttpResponse
        {
            StatusCode = System.Net.HttpStatusCode.NotFound,
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
            StatusCode = System.Net.HttpStatusCode.OK,
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
            StatusCode = System.Net.HttpStatusCode.OK,
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "text/plain" },
            },
            BodyAsString = "warmup",
        };
    }

    public static HttpResponse GetUuidHandler(HttpRequest request)
    {
        var uuidRequest = new HttpRequest
        {
            Method = Fermyon.Spin.Sdk.HttpMethod.Get,
            Url = "https://httpbin.org/uuid",
            Headers = HttpKeyValues.FromDictionary(new Dictionary<string, string>
            {
                { "Accept", "application/json" },
            })
        };

        HttpResponse uuidResponse = default;

        try
        {
            uuidResponse = OutboundServices.SendHttpRequest(uuidRequest);

        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Uuid failed: {ex.Message}");
            return new HttpResponse
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };
        }

        var status = uuidResponse.StatusCode;
        Logger.LogInformation($"Uuid status code: {status}");

        var body = uuidResponse.BodyAsString;

        return new HttpResponse
        {
            StatusCode = System.Net.HttpStatusCode.OK,
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
            },
            BodyAsString = body,
        };
    }

}
