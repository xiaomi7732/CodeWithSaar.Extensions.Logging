namespace CodeWithSaar.Extensions.Logging.File
{
    /// <summary>
    /// A stub implementation for scope.
    /// </summary>
    internal sealed class NullScopeImp : IDisposable
    {
        private NullScopeImp()
        {
        }
        public static NullScopeImp Instance { get; } = new NullScopeImp();

        public void Dispose()
        {
        }
    }
}