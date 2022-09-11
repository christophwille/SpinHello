using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AzFunc4Comparison.Startup))]

namespace AzFunc4Comparison
{
    // See https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddLogging();  // required when going from static to DI in ctor

            //builder.Services.AddSingleton<IMyService>((s) => {
            //    return new MyService();
            //});
            //builder.Services.AddSingleton<ILoggerProvider, MyLoggerProvider>();
        }
    }
}
