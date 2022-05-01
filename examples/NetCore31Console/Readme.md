# Example for file logging provider in worker for .NET Core 3.1 application

This is an example for file logging provider working in worker service in .NET Core 3.1 application. This is also used as a manual test to making sure the library keeps working for 3.x packages.

## To run the app

```shell
cd examples\NetCore31Console
dotnet run
```

## Expected output

```log
[2022-05-01T19:21:00.1492033Z, NetCore31Console.WorkerService, Information] Logging info!
[2022-05-01T19:21:00.1515449Z, Microsoft.Hosting.Lifetime, Information] Application started. Press Ctrl+C to shut down.
[2022-05-01T19:21:00.1533541Z, Microsoft.Hosting.Lifetime, Information] Hosting environment: Production
[2022-05-01T19:21:00.1533918Z, Microsoft.Hosting.Lifetime, Information] Content root path: D:\Repos\CodeWithSaar.Extensions.Logging\examples\NetCore31Console
[2022-05-01T19:21:05.1617754Z, NetCore31Console.WorkerService, Information] Logging info!
[2022-05-01T19:21:10.1709596Z, NetCore31Console.WorkerService, Information] Logging info!
[2022-05-01T19:21:11.3513217Z, Microsoft.Hosting.Lifetime, Information] Application is shutting down...
```
