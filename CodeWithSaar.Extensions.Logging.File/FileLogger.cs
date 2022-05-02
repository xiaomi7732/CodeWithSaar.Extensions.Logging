using System.Text;
using Microsoft.Extensions.Logging;

namespace CodeWithSaar.Extensions.Logging.File;

internal sealed class FileLogger : ILogger, IDisposable
{
    private bool _isDisposed = false;
    private readonly string _categoryName;
    private readonly FileLoggerFormatter _formatter;
    private readonly ILoggerWriter _loggerWriter;

    public FileLogger(
        string categoryName,
        FileLoggerFormatter formatter,
        ILoggerWriter loggerWriter)
    {
        if (string.IsNullOrEmpty(categoryName))
        {
            throw new ArgumentException($"'{nameof(categoryName)}' cannot be null or empty.", nameof(categoryName));
        }

        _categoryName = categoryName;
        _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        _loggerWriter = loggerWriter ?? throw new ArgumentNullException(nameof(loggerWriter));
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        // Scope is not supported... yet
        return NullScopeImp.Instance;
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }
        _isDisposed = true;

        t_stringWriter?.Dispose();
        if (_loggerWriter is IDisposable disposableLogWriter)
        {
            disposableLogWriter.Dispose();
        }
    }

    [ThreadStatic]
    private static StringWriter? t_stringWriter;

    public bool IsEnabled(LogLevel logLevel)
        => logLevel != LogLevel.None;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        if (formatter is null)
        {
            throw new ArgumentNullException(nameof(formatter));
        }

        t_stringWriter ??= new StringWriter();
        LogEntry<TState> logEntry = new LogEntry<TState>(logLevel, _categoryName, eventId, state, exception, formatter);
        _formatter.Write(in logEntry, t_stringWriter);

        StringBuilder sb = t_stringWriter.GetStringBuilder();
        if (sb.Length == 0)
        {
            return;
        }
        string computedAnsiString = sb.ToString();
        sb.Clear();
        if (sb.Capacity > 1024)
        {
            sb.Capacity = 1024;
        }
        _loggerWriter.WriteContent(computedAnsiString);
    }
}