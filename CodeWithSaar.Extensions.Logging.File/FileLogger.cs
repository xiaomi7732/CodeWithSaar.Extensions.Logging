using Microsoft.Extensions.Logging;

namespace CodeWithSaar.Extensions.Logging.File;

public sealed class FileLogger : ILogger, IDisposable
{
    private bool _isDisposed = false;
    private readonly string _categoryName;
    private readonly Func<FileLoggerOptions> _getOptions;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public FileLogger(string categoryName, Func<FileLoggerOptions> getOptions)
    {
        if (string.IsNullOrEmpty(categoryName))
        {
            throw new ArgumentException($"'{nameof(categoryName)}' cannot be null or empty.", nameof(categoryName));
        }

        _categoryName = categoryName;
        _getOptions = getOptions ?? throw new ArgumentNullException(nameof(getOptions));
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        // Scope is not supported... yet
        return new ScopeStub();
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }
        _isDisposed = true;
        _semaphore.Dispose();
    }

    public bool IsEnabled(LogLevel logLevel)
        => logLevel != LogLevel.None;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string fileName = _getOptions().OutputFilePath;

        _semaphore.Wait();
        try
        {
            using (Stream outputStream = System.IO.File.Open(fileName, FileMode.Append))
            using (StreamWriter streamWriter = new StreamWriter(outputStream))
            {
                streamWriter.Write($"[{_categoryName}, {logLevel}] ");
                streamWriter.WriteLine(formatter(state, exception));
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }
}