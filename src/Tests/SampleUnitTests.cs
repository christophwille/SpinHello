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
}