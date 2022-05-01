namespace CodeWithSaar.Extensions.Logging.File
{
    public interface ILoggerWriter
    {
        void WriteContent(string line);
    }
}