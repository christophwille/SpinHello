using Microsoft.Extensions.Logging;

namespace SpinHello
{
    // https://docs.microsoft.com/en-us/dotnet/core/extensions/custom-logging-provider
    internal class SpinLogger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state) => default!;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (null != exception)
            {
                Console.Error.WriteLine($"{formatter(state, exception)}");
                Console.Error.WriteLine(exception.ToString());
            }
            else
            {
                Console.WriteLine($"{formatter(state, exception)}");
            }
        }
    }
}
