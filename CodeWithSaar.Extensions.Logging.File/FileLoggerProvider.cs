using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CodeWithSaar.Extensions.Logging.File;

[ProviderAlias("FileProvider")]
public sealed class FileLoggerProvider : ILoggerProvider
{
    private bool _isDisposed;
    private FileLoggerOptions _currentOptions;
    private readonly IDisposable? _updateToken;

    private readonly FileLoggerWriter _fileLoggerWriter;
    private readonly ConcurrentDictionary<string, ILogger> _loggers =
    new ConcurrentDictionary<string, ILogger>(StringComparer.OrdinalIgnoreCase);

    public FileLoggerProvider(IOptionsMonitor<FileLoggerOptions> options)
    {
        _currentOptions = options?.CurrentValue ?? new FileLoggerOptions();
        _updateToken = options?.OnChange(updated => _currentOptions = updated);
        
        _fileLoggerWriter = FileLoggerWriter.Instance.WithOptions(GetCurrentOptions);
        
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, key => new FileLogger(
            categoryName,
            new FileLoggerFormatter(GetCurrentOptions),
            _fileLoggerWriter));
    }

    private FileLoggerOptions GetCurrentOptions() => _currentOptions;

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;
        _updateToken?.Dispose();
        _fileLoggerWriter.Dispose();
    }
}