using BenchmarkDotNet.Running;
using CodeWithSaar.Extensions.Logging.File.Benchmark;

BenchmarkRunner.Run<FileLoggerBenchmark>();

// FileLoggerBenchmark benchmarkDebug = new FileLoggerBenchmark();
// var x = benchmarkDebug.Log();
// Console.WriteLine(x);
// benchmarkDebug.CleanUp();
