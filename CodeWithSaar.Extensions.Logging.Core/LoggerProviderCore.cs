
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace CodeWithSaar.Extensions.Logging.Android;
public sealed class LoggerProviderCore : ILoggerProvider
{
    private Action<LogLevel, EventId, object?, Exception?, Func<object, Exception?, string>>? _writeLog;

    private readonly ConcurrentDictionary<string, LoggerCore> _loggers = new ConcurrentDictionary<string, LoggerCore>(StringComparer.OrdinalIgnoreCase);

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, (name) =>
        {
            if (_writeLog is null)
            {
                return new LoggerCore((level, eventId, state, ex, formatter) =>
                {
                    Console.WriteLine("Invalid initializing {0}. Call WithWriter on {1} first", nameof(LoggerCore), nameof(LoggerProviderCore));
                });
            }
            return new LoggerCore(_writeLog);
        });
    }

    public LoggerProviderCore WithWriter(Action<LogLevel, EventId, object?, Exception?, Func<object, Exception?, string>> writeLog)
    {
        _writeLog = writeLog;
        return this;
    }

    public void Dispose()
    {
        _loggers.Clear();
    }
}
