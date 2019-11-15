using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace RLog
{
    public class LogRProvider : ILoggerProvider
    {
        private readonly RLogConfigurator _logConfigurator;

        public LogRProvider(RLogConfigurator logConfigurator) => _logConfigurator = logConfigurator;

        private ConcurrentDictionary<string, Logger> Loggers { get; set; } = new ConcurrentDictionary<string, Logger>();

        public ILogger CreateLogger(string categoryName)
        {
            Logger customLogger = Loggers.GetOrAdd(categoryName, new Logger(LogContextProvider.Instance.CreateLogContext(categoryName), _logConfigurator.GetLogDistributor()));
            return customLogger;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~LogRProvider()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
