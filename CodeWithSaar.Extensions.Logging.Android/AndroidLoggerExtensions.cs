using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace CodeWithSaar.Extensions.Logging.Android
{
    public static class AndroidLoggerExtensions
    {
        public static ILoggingBuilder AddColorConsoleLogger(
            this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, AndroidLoggerProvider>());

            //LoggerProviderOptions.RegisterProviderOptions
            //    <ColorConsoleLoggerConfiguration, ColorConsoleLoggerProvider>(builder.Services);

            return builder;
        }
    }
}