using Moq;
using Newtonsoft.Json;
using SpinHello;

namespace Tests;

public class DefaultHandlerTests
{
    [Fact]
    public void VerifyWarmup()
    {
        var request = new HttpRequest()
        {
            Url = Warmup.DefaultWarmupUrl
        };

        var result = Handler.HandleHttpRequest(request);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal("warmup", result.BodyAsString);
    }

    [Fact]
    public void HelloWorldTest()
    {
        var request = new HttpRequest()
        {
            Url = "/hello"
        };

        var result = Handler.HandleHttpRequest(request);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal("Hello from .NET\n", result.BodyAsString);
    }

    [Fact]
    public void NotFoundTest()
    {
        var request = new HttpRequest()
        {
            Url = "/thisurlforsuredoesntexist"
        };

        var result = Handler.HandleHttpRequest(request);

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

    record UuidResponse(string uuid);
    [Fact]
    public void MockingOutboundHttpRequestsTest()
    {
        string fakeUuid = "dfa4a1da-285e-48d4-81f1-d24a92150232";
        string fakeUuidJson = JsonConvert.SerializeObject(new UuidResponse(fakeUuid));

        var request = new HttpRequest()
        {
            Url = "/uuid"
        };

        var commsMock = new Mock<IOutboundCommunication>();
        commsMock.Setup(m => m.SendHttpRequest(It.IsAny<HttpRequest>()))
                    .Returns(new HttpResponse()
                    {
                        StatusCode = HttpStatusCode.OK,
                        BodyAsString = fakeUuidJson
                    });

        Handler.OutboundServices = commsMock.Object;
        var result = Handler.HandleHttpRequest(request);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        var uuidResponseParsed = JsonConvert.DeserializeObject<UuidResponse>(result.BodyAsString);
        Assert.Equal(fakeUuid, uuidResponseParsed.uuid);
    }
}