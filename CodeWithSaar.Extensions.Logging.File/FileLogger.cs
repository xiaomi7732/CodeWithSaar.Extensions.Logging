using Microsoft.Extensions.Logging;

namespace CodeWithSaar.Extensions.Logging.File;

public class FileLogger : ILogger
{
    private readonly string _categoryName;
    private readonly Func<FileLoggerOptions> _getOptions;

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

    public bool IsEnabled(LogLevel logLevel)
        => logLevel != LogLevel.None;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string fileName = _getOptions().OutputFilePath;

        using (Stream outputStream = System.IO.File.Open(fileName, FileMode.Append))
        using (StreamWriter streamWriter = new StreamWriter(outputStream))
        {
            streamWriter.Write($"[{_categoryName}, {logLevel}] ");
            streamWriter.WriteLine(formatter(state, exception));
        }
    }
}