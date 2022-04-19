using System.Text;
using System.Threading.Channels;

namespace CodeWithSaar.Extensions.Logging.File;

internal sealed class FileLoggerWriter : ILoggerWriter, IDisposable
{
    private const int DefaultFileStreamBufferSize = 4096;
    private bool _isDisposed = false;
    private string _lastUsedFileName = string.Empty;
    private StreamWriter? _loggingWriter = null;
    private readonly Channel<string> _channel;
    private Func<FileLoggerOptions> _getOptions;
    private static readonly object _switchFileLocker = new object();

    private FileLoggerWriter()
    {
        _channel = Channel.CreateUnbounded<string>(new UnboundedChannelOptions()
        {
            SingleReader = true,
            AllowSynchronousContinuations = false,
        });

        _getOptions = () => new FileLoggerOptions();

        Task.Run(async () =>
        {
            try
            {
                await RealizeLogAsync(default).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // This shall never happen
                string message = "Unexpected exception happened in FileLogger Provider. " + ex.ToString();
                if (_loggingWriter is not null)
                {
                    await _loggingWriter.WriteLineAsync(message).ConfigureAwait(false);
                }
                else
                {
                    // Just in case:
                    Console.WriteLine(message);
                }
            }
        });
    }
    public static FileLoggerWriter Instance { get; } = new FileLoggerWriter();

    public FileLoggerWriter WithOptions(
        Func<FileLoggerOptions> getOptions)
    {
        _getOptions = getOptions ?? throw new ArgumentNullException(nameof(getOptions));
        return this;
    }

    public void WriteLine(string line)
    {
        _channel.Writer.TryWrite(line);
    }

    private async Task RealizeLogAsync(CancellationToken cancellationToken)
    {
        while (await _channel.Reader.WaitToReadAsync(cancellationToken).ConfigureAwait(false))
        {
            string line = await _channel.Reader.ReadAsync().ConfigureAwait(false);
            EnsureFilePathCurrent();
            if (_loggingWriter is not null)
            {
                await _loggingWriter.WriteLineAsync(line);
            }
        }
    }

    private void EnsureFilePathCurrent()
    {
        FileLoggerOptions currentOptions = _getOptions();
        string currentFileName = currentOptions.OutputFilePath;
        bool autoFlush = currentOptions.AutoFlush;

        // New file
        if (string.IsNullOrEmpty(_lastUsedFileName))
        {
            lock (_switchFileLocker)
            {
                Stream outputStream = System.IO.File.Open(currentFileName, FileMode.Append, FileAccess.Write, FileShare.Read);
                _loggingWriter = new StreamWriter(outputStream, Encoding.UTF8, DefaultFileStreamBufferSize, leaveOpen: false);
                _loggingWriter.AutoFlush = autoFlush;
            }
            _lastUsedFileName = currentFileName;
            return;
        }

        // Existing file
        if (!string.Equals(_lastUsedFileName, currentFileName))
        {
            // File name has since changed;
            lock (_switchFileLocker)
            {
                Stream outputStream = System.IO.File.Open(currentFileName, FileMode.Append, FileAccess.Write, FileShare.Read);
                _loggingWriter?.Dispose();
                _loggingWriter = new StreamWriter(outputStream, Encoding.UTF8, DefaultFileStreamBufferSize, leaveOpen: false);
                _loggingWriter.AutoFlush = autoFlush;
                _lastUsedFileName = currentFileName;
            }
            return;
        }

        // Otherwise, keep what we have.
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;
        _channel.Writer.TryComplete();
        _loggingWriter?.Dispose();

    }
}