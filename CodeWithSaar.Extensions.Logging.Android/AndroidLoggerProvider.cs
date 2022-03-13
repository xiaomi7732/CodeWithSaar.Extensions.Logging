#nullable enable

using Android.Util;
using Microsoft.Extensions.Logging;
using System;

namespace CodeWithSaar.Extensions.Logging.Android
{
    [ProviderAlias("ColorConsole")]
    public class AndroidLoggerProvider : LoggerProviderBase
    {
        protected override void WriteLog(string categoryName, LogLevel logLevel, EventId eventId, object? state, Exception? exception, Func<object, Exception?, string> formatter)
        {
            if (state is null)
            {
                throw new InvalidOperationException("State can't be null");
            }

            Func<string?, string, int>? invokeLog;
            switch (logLevel)
            {
                case LogLevel.Trace:
                    invokeLog = Log.Verbose;
                    break;
                case LogLevel.Debug:
                    invokeLog = Log.Debug;
                    break;
                case LogLevel.Information:
                    invokeLog = Log.Info;
                    break;
                case LogLevel.Warning:
                    invokeLog = Log.Warn;
                    break;
                case LogLevel.Error:
                    invokeLog = Log.Error;
                    break;
                case LogLevel.Critical:
                    invokeLog = Log.Wtf;
                    break;
                case LogLevel.None:
                default:
                    invokeLog = null;
                    break;
            }

            invokeLog?.Invoke(categoryName, formatter(state, exception));
        }
    }
}