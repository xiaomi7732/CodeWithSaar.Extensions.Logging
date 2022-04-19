namespace CodeWithSaar.Extensions.Logging.File;

/// <summary>
/// Options for File Logging Provider.
/// </summary>
public sealed class FileLoggerOptions
{
    /// <summary>
    /// Gets or sets the output file path.
    /// </summary>
    public string OutputFilePath { get; set; } = "output.log";

    /// <summary>
    /// Gets or sets whether to show full category name. Default to true.
    /// </summary>
    public bool ShowFullCategoryName { get; set; } = true;

    /// <summary>
    /// Gets or sets the timestamp format. Default to yyyy-MM-dd HH:mm:ss
    /// </summary>
    public string TimestampFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";

    /// <summary>
    /// Gets or sets whether to use UTC for timestamp or not.
    /// </summary>
    /// <value></value>
    public bool UseUTCTimestamp { get; set; } = false;

    /// <summary>
    /// Gets or sets whether to auto flush logs. Set true for accessing logs quicker; false for better performance.
    /// </summary>
    public bool AutoFlush { get; set; } = true;
}