using Microsoft.Extensions.Logging;

namespace CodeWithSaar.Extensions.Logging.File;

internal sealed class FileLogger : ILogger, IDisposable
{
    private bool _isDisposed = false;
    private readonly string _categoryName;
    private readonly Func<FileLoggerOptions> _getOptions;
    private readonly ILoggerWriter _loggerWriter;

    public FileLogger(
        string categoryName,
        Func<FileLoggerOptions> getOptions,
        ILoggerWriter loggerWriter)
    {
        if (string.IsNullOrEmpty(categoryName))
        {
            throw new ArgumentException($"'{nameof(categoryName)}' cannot be null or empty.", nameof(categoryName));
        }

        _categoryName = categoryName;
        _getOptions = getOptions ?? throw new ArgumentNullException(nameof(getOptions));
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
    }

    public bool IsEnabled(LogLevel logLevel)
        => logLevel != LogLevel.None;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        FileLoggerOptions currentOptions = _getOptions();
        string message;
        List<string> prefix = new List<string>();
        if (!string.IsNullOrEmpty(currentOptions.TimestampFormat))
        {
            prefix.Add(DateTime.Now.ToString(currentOptions.TimestampFormat));
        }
        if (currentOptions.ShowFullCategoryName)
        {
            prefix.Add(_categoryName);
        }
        prefix.Add(logLevel.ToString());
        message = string.Format("[{0}] {1}", string.Join(", ", prefix), formatter(state, exception));
        _loggerWriter.WriteLine(message);
    }
}