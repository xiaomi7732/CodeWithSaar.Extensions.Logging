using Microsoft.Extensions.Logging;

namespace CodeWithSaar.Extensions.Logging.File;

internal class FileLoggerFormatter
{
    private readonly Func<FileLoggerOptions> _getOptions;
    private const string LoglevelPadding = ": ";
    private static readonly string _messagePadding = new string(' ', GetLogLevelString(LogLevel.Information).Length + LoglevelPadding.Length);
    private static readonly string _newLineWithMessagePadding = Environment.NewLine + _messagePadding;

    public FileLoggerFormatter(Func<FileLoggerOptions> getOptions)
    {
        this._getOptions = getOptions ?? throw new System.ArgumentNullException(nameof(getOptions));
    }

    /// <summary>
    /// Writes the log message to the specified TextWriter.
    /// </summary>
    /// <remarks>
    /// if the formatter wants to write colors to the console, it can do so by embedding ANSI color codes into the string
    /// </remarks>
    /// <param name="logEntry">The log entry.</param>
    /// <param name="textWriter">The string writer embedding ansi code for colors.</param>
    /// <typeparam name="TState">The type of the object to be written.</typeparam>
    public void Write<TState>(in LogEntry<TState> logEntry, TextWriter textWriter)
    {
        string message = logEntry.Formatter(logEntry.State, logEntry.Exception);
        if (logEntry.Exception is null && message is null)
        {
            return;
        }
        LogLevel logLevel = logEntry.LogLevel;
        string logLevelString = GetLogLevelString(logLevel);

        string? timestamp = null;
        string? timestampFormat = _getOptions().TimestampFormat;
        if (!string.IsNullOrEmpty(timestampFormat))
        {
            DateTime dateTime = GetCurrentDateTime();
            timestamp = dateTime.ToString(timestampFormat).TrimEnd() + " ";
        }
        if (timestamp != null)
        {
            textWriter.Write(timestamp);
        }
        if (logLevelString != null)
        {
            textWriter.Write(logLevelString);
        }
        CreateDefaultLogMessage(textWriter, logEntry, message);
    }

    private void CreateDefaultLogMessage<TState>(TextWriter textWriter, in LogEntry<TState> logEntry, string message)
    {
        bool singleLine = true;
        int eventId = logEntry.EventId.Id;
        FileLoggerOptions options = _getOptions();
        bool fullCategoryName = options.ShowFullCategoryName;
        bool autoFlush = options.AutoFlush;
        Exception? exception = logEntry.Exception;

        // Example:
        // info: ConsoleApp.Program[10]
        //       Request received

        // category and event id
        textWriter.Write(LoglevelPadding);

        if (fullCategoryName)
        {
            textWriter.Write(logEntry.Category);
        }
        textWriter.Write('[');

#if NETCOREAPP
        Span<char> span = stackalloc char[10];
        if (eventId.TryFormat(span, out int charsWritten))
            textWriter.Write(span.Slice(0, charsWritten));
        else
#endif
        textWriter.Write(eventId.ToString());

        textWriter.Write(']');
        if (!singleLine)
        {
            textWriter.Write(Environment.NewLine);
        }

        // scope information
        WriteMessage(textWriter, message, singleLine);

        // Example:
        // System.InvalidOperationException
        //    at Namespace.Class.Function() in File:line X
        if (exception != null)
        {
            // exception message
            WriteMessage(textWriter, exception.ToString(), singleLine);
        }
        if (singleLine)
        {
            textWriter.Write(Environment.NewLine);
        }
        
        if(autoFlush)
        {
            textWriter.Flush();
        }
    }

    private static void WriteMessage(TextWriter textWriter, string message, bool singleLine)
    {
        if (!string.IsNullOrEmpty(message))
        {
            if (singleLine)
            {
                textWriter.Write(' ');
                WriteReplacing(textWriter, Environment.NewLine, " ", message);
            }
            else
            {
                textWriter.Write(_messagePadding);
                WriteReplacing(textWriter, Environment.NewLine, _newLineWithMessagePadding, message);
                textWriter.Write(Environment.NewLine);
            }
        }

        static void WriteReplacing(TextWriter writer, string oldValue, string newValue, string message)
        {
            string newMessage = message.Replace(oldValue, newValue);
            writer.Write(newMessage);
        }
    }

    private DateTime GetCurrentDateTime()
    {
        return _getOptions().UseUTCTimestamp ? DateTime.UtcNow : DateTime.Now;
    }

    private static string GetLogLevelString(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => "trce",
            LogLevel.Debug => "dbug",
            LogLevel.Information => "info",
            LogLevel.Warning => "warn",
            LogLevel.Error => "fail",
            LogLevel.Critical => "crit",
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel))
        };
    }
}