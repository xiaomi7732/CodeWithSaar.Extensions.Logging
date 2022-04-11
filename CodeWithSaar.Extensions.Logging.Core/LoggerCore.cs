using Microsoft.Extensions.Logging;

namespace CodeWithSaar.Extensions.Logging.Core;
public sealed class LoggerCore : ILogger
{
    private readonly Action<LogLevel, EventId, object?, Exception?, Func<object, Exception?, string>> _writeLog;
    private readonly Func<LogLevel, bool> _isEnabled;

    public LoggerCore(
        Action<LogLevel, EventId, object?, Exception?, Func<object, Exception?, string>> writeLog,
        Func<LogLevel, bool>? isEnabled = null)
    {
        _writeLog = writeLog ?? throw new ArgumentNullException(nameof(writeLog));
        _isEnabled = isEnabled ?? (level => level != LogLevel.None);
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        throw new NotImplementedException();
    }

    public bool IsEnabled(LogLevel logLevel)
        => _isEnabled(logLevel);

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }
        _writeLog(logLevel, eventId, state, exception, (obj, ex) => formatter((TState)obj, ex));
    }
}