using CodeWithSaar.Extensions.Logging.File;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;

namespace Microsoft.Extensions.Logging;

public static class FileLoggerExtension
{
    public static ILoggingBuilder AddFile(this ILoggingBuilder builder)
    {
        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, FileLoggerProvider>()
        );

        LoggerProviderOptions.RegisterProviderOptions
            <FileLoggerOptions, FileLoggerProvider>(builder.Services);

        return builder;
    }

    public static ILoggingBuilder AddFile(this ILoggingBuilder builder, Action<FileLoggerOptions> configure)
    {
        builder.AddFile();
        builder.Services.Configure(configure);
        return builder;
    }
}