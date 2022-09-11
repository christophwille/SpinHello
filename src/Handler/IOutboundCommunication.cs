using Fermyon.Spin.Sdk;

namespace SpinHello;

public interface IOutboundCommunication
{
    HttpResponse SendHttpRequest(HttpRequest request);
}

internal class DefaultOutboundCommunication : IOutboundCommunication
{
    public HttpResponse SendHttpRequest(HttpRequest request)
    {
        return HttpOutbound.Send(request);
    }
}
