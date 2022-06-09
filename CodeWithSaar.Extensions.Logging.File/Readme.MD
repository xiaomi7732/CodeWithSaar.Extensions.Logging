# CodeWithSaar.Extensions.Logging.File

[![Build example](https://github.com/xiaomi7732/CodeWithSaar.Extensions.Logging/actions/workflows/nightly-example.yaml/badge.svg)](https://github.com/xiaomi7732/CodeWithSaar.Extensions.Logging/actions/workflows/nightly-example.yaml) ![Package-Badge](https://img.shields.io/nuget/v/CodeWithSaar.Extensions.Logging.File?style=plastic) ![Download-Badge](https://img.shields.io/nuget/dt/CodeWithSaar.Extensions.Logging.File?style=plastic)

`CodeWithSaar.Extensions.Logging.File` is a very easy to use, lightweight file logger provider implementation for Microsoft.Extensions.Logging. Start to use it with ASP.NET Core by one line of code.

## Getting Started

* Add [NuGet Package](https://www.nuget.org/packages/CodeWithSaar.Extensions.Logging.File):

    ```csharp
    dotnet add package CodeWithSaar.Extensions.Logging.File
    ```

* In ASP.NET Core project, enable the logging provider:

    ```csharp
    ...
    // Add services to the container.
    builder.Services.AddLogging(loggingBuilder =>{
        loggingBuilder.AddFile();
    });
    ...
    ```

    Check out the [example projects](https://github.com/xiaomi7732/CodeWithSaar.Extensions.Logging/tree/main/examples/) for Console or other types of project.

* Run the application and see the log output to `output.log`, and you are expected to see log entries like this:

    ```log
    2022-05-01T15:01:06.6811783-07:00 dbug: Microsoft.Extensions.Hosting.Internal.Host[1] Hosting starting
    2022-05-01T15:01:09.7265572-07:00 info: Microsoft.Hosting.Lifetime[14] Now listening on: https://localhost:7038
    2022-05-01T15:01:09.7269258-07:00 info: Microsoft.Hosting.Lifetime[14] Now listening on: http://localhost:5125
    2022-05-01T15:01:09.7281242-07:00 info: Microsoft.Hosting.Lifetime[0] Application started. Press Ctrl+C to shut down.
    2022-05-01T15:01:09.7283214-07:00 info: Microsoft.Hosting.Lifetime[0] Hosting environment: Development
    2022-05-01T15:01:09.7283445-07:00 info: Microsoft.Hosting.Lifetime[0] Content root path: D:\Repos\CodeWithSaar.Extensions.Logging\examples\WebAPIExample\
    2022-05-01T15:01:09.7283522-07:00 dbug: Microsoft.Extensions.Hosting.Internal.Host[2] Hosting started
    2022-05-01T15:01:13.9569295-07:00 info: Microsoft.Hosting.Lifetime[0] Application is shutting down...
    2022-05-01T15:01:13.9580170-07:00 dbug: Microsoft.Extensions.Hosting.Internal.Host[3] Hosting stopping
    2022-05-01T15:01:13.9705049-07:00 dbug: Microsoft.Extensions.Hosting.Internal.Host[4] Hosting stopped
    ```

## Benchmark Result

``` ini
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Core i7-9700 CPU 3.00GHz, 1 CPU, 8 logical and 8 physical cores
.NET SDK=6.0.300-preview.22204.3
  [Host]     : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
  DefaultJob : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
```

| Method |     Mean |    Error |   StdDev |
|------- |---------:|---------:|---------:|
|    Log | 750.3 ns | 14.14 ns | 13.22 ns |

## Example projects

* [WebAPI (ASP.NET Core 6 / .NET 6)](https://github.com/xiaomi7732/CodeWithSaar.Extensions.Logging/tree/main/examples/WebAPIExample)
* [Console / Worker (.NET Core 3.1)](https://github.com/xiaomi7732/CodeWithSaar.Extensions.Logging/tree/main/examples/NetCore31Console)

## Customization

### Set a different file name

There are generally 2 ways to set log configurations.

* By code:

    ```csharp
    builder.Services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.AddFile(opt => opt.OutputFilePath = "newfilename.log");
    });
    ```

* By configuration file (Typically, appsettings.json):

    ```jsonc
    {
        // General logging
        "Logging": {
            "LogLevel": {
                "Default": "Debug"
            },
            // File provider overwrites
            "FileProvider":
            {
                // Output to output.v2.log file
                "OutputFilePath": "output.v2.log",
                // Specify a different logging level
                "LogLevel":{
                    "Default": "Information"
                }
            }
        }
    }
    ```

### Set different logging levels

Logging levels could be set in through `appsettings.json` / `appsettings.Development.json` or whatever the configuration providers in your project.

It would take the default settings like this:

```jsonc
    {
        // Logging Information or above to file
        "Logging": {
            "LogLevel": {
                "Default": "Debug"
            }
        }
    }
```

Or it could be set to it's own:

```jsonc
{
    // Default logging level of Debug
    "Logging": {
        "LogLevel": {
            "Default": "Debug"
        },
        "FileProvider":
        {
            // Output to output.v2.log file
            "OutputFilePath": "output.v2.log",
            // Specify a different logging level of Information for logs output to the file.
            "LogLevel":{
                "Default": "Information"
            }
        }
    }
}
```

### Other customizations

There are more options than just the filename. All supports the settings from either the delegate in code or the configuration providers (like it in appsettings.json). Refer to [FileLoggerOptions.cs](https://github.com/xiaomi7732/CodeWithSaar.Extensions.Logging/blob/main/CodeWithSaar.Extensions.Logging.File/FileLoggerOptions.cs) for more properties to be customized like: `ShowFullCategoryName`, `TimestampFormat`, etc.

### Others

Please star this repo if you like this repo :-).

Want to know how is this built? Check out the following videos:

* [How to implement a custom logging provider for files (1/2)](https://youtu.be/3RUpYR4dZM4)
* [How to implement a custom logging provider for files (2/2)](https://youtu.be/3Z3yemyikNc)

## Issues

* If you want features or encountered any problem, file an [issue](https://github.com/xiaomi7732/CodeWithSaar.Extensions.Logging/issues).

## Disclosure

* I am current working for Microsoft.
* This is NOT a Microsoft project.
