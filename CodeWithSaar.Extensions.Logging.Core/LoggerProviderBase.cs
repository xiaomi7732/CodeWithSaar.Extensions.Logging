
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace CodeWithSaar.Extensions.Logging.Core;
public abstract class LoggerProviderBase : ILoggerProvider, IDisposable
{
    private readonly ConcurrentDictionary<string, LoggerCore> _loggers = new ConcurrentDictionary<string, LoggerCore>(StringComparer.OrdinalIgnoreCase);
    private bool _disposed = false;

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, (name) =>
        {
            return new LoggerCore((level, eventId, state, exception, formatter) =>
            {
                WriteLog(categoryName, level, eventId, state, exception, formatter);
            });
        });
    }

    protected abstract void WriteLog(string categoryName, LogLevel logLevel, EventId eventId, object? state, Exception? exception, Func<object, Exception?, string> formatter);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool isDisposing)
    {
        if (_disposed)
        {
            return;
        }
        _disposed = true;

        if (isDisposing)
        {
            _loggers.Clear();
        }
    }
}
