using System.Collections.Concurrent;

namespace RLog
{
    public sealed class LogContextProvider
    {
        private LogContextProvider() { }

        private static LogContextProvider? s_instance;

        public static LogContextProvider Instance => s_instance ??= new LogContextProvider();

        private ConcurrentDictionary<string, LogContext> LogContexts { get; } = new ConcurrentDictionary<string, LogContext>();

        public LogContext CreateLogContext(string categoryName) => LogContexts.GetOrAdd(categoryName, new LogContext(categoryName));
    }
}
