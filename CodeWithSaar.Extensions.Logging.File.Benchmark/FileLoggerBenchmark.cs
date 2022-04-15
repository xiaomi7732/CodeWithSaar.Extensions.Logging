using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging;

namespace CodeWithSaar.Extensions.Logging.File.Benchmark;

public class FileLoggerBenchmark
{
    FileLogger _logger;
    IDisposable _logWriter;
    public FileLoggerBenchmark()
    {
        FileLoggerWriter writer = FileLoggerWriter.Instance;
        _logWriter = writer;
        _logger = new FileLogger("Benchmark", () => new FileLoggerOptions(), writer);
    }

    [Benchmark]
    public int Log()
    {
        string message = "Hello";
        _logger.LogInformation(message);
        return message.Length;
    }

    [GlobalCleanup(Target = nameof(Log))]
    public void CleanUp()
    {
        _logger.Dispose();
        _logWriter.Dispose();
    }
}