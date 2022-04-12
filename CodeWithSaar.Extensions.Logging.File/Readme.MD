# CodeWithSaar.Extensions.Logging.File

A file logger provider implementation for Microsoft.Extensions.Logging. At this moment, this package is very easy to use in debugging environment.

Refer to the [Known-limitations](#Known-limitations) before you use this in production.

## Getting Started

* Add NuGet Package:

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

* Run the application and see the log output to `output.log`.

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

    ```json
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

### Set 

## Known limitations

* Scope is not supported yet;
* File writing performance is not optimized;