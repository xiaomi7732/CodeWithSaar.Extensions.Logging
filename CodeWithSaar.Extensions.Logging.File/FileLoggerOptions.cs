namespace CodeWithSaar.Extensions.Logging.File;

public sealed class FileLoggerOptions
{
    public string OutputFilePath { get; set; } = "output.log";
    public bool ShowFullCategoryName { get; set; } = true;
    public string TimestampFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";
}