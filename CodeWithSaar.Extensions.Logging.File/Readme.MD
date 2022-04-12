# CodeWithSaar.Extensions.Logging.File

A very easy to use file logger provider implementation for Microsoft.Extensions.Logging.

Refer to the [Known-limitations](#Known-limitations) before you use this in production environment.

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

    Check out the [example project](../examples/WebAPIExample/).

* Run the application and see the log output to `output.log`, and you are expected to see log entries like this:

    ```csharp
    [2022-04-12 12:12:42, Microsoft.Extensions.Hosting.Internal.Host, Debug] Hosting starting
    [2022-04-12 12:12:42, Microsoft.Hosting.Lifetime, Information] Now listening on: https://localhost:7038
    [2022-04-12 12:12:42, Microsoft.Hosting.Lifetime, Information] Now listening on: http://localhost:5125
    [2022-04-12 12:12:42, Microsoft.Hosting.Lifetime, Information] Application started. Press Ctrl+C to shut down.
    [2022-04-12 12:12:42, Microsoft.Hosting.Lifetime, Information] Hosting environment: Development
    [2022-04-12 12:12:42, Microsoft.Hosting.Lifetime, Information] Content root path: D:\Repos\CodeWithSaar.Extensions.Logging\examples\WebAPIExample\
    [2022-04-12 12:12:42, Microsoft.Extensions.Hosting.Internal.Host, Debug] Hosting started
    [2022-04-12 12:12:44, Microsoft.Hosting.Lifetime, Information] Application is shutting down...
    [2022-04-12 12:12:44, Microsoft.Extensions.Hosting.Internal.Host, Debug] Hosting stopping
    [2022-04-12 12:12:44, Microsoft.Extensions.Hosting.Internal.Host, Debug] Hosting stopped
    ```

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

## Known limitations

* Scope needs to be implemented;
* Writing performance needs to be benchmarked;
