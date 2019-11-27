using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace RLog.Outputs.Distribution
{
    public class SerialDistributor : ILogDistributor
    {
        private readonly IEnumerable<ILogOutput> _logOutputs;

        public SerialDistributor(IEnumerable<ILogOutput> logOutputs) => _logOutputs = logOutputs;

        public void Push(LogLevel logLevel, LogContext logContext, string msg)
        {
            foreach (ILogOutput logOutput in _logOutputs)
            {
                logOutput.Write(logLevel, logContext, msg);
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            foreach (ILogOutput logOutput in _logOutputs)
            {
                if (logOutput.IsEnabled(logLevel))
                {
                    return true;
                }
            }

            return false;
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
                    foreach (ILogOutput output in _logOutputs)
                    {
                        if (output is IDisposable disposable)
                        {
                            disposable.Dispose();
                        }
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SerialDistributor()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() =>
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);// TODO: uncomment the following line if the finalizer is overridden above.// GC.SuppressFinalize(this);
        #endregion
    }
}
