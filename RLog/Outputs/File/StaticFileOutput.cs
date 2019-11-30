using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace RLog.Outputs.File
{
    public class StaticFileOutput : ILogOutput, IDisposable
    {
        private readonly FileStream _fileStream;
        private readonly StreamWriter _writer;
        private readonly GlobalContext _globalContext;
        private readonly string _logPath;
        private readonly LogLevel _minLevel;

        public StaticFileOutput(GlobalContext globalContext, string logPath, LogLevel minLevel)
        {
            _globalContext = globalContext;
            _logPath = logPath;
            _minLevel = minLevel;

            _fileStream = new FileStream(globalContext.Format(logPath), FileMode.Append, FileAccess.Write, FileShare.Read);
            _writer = new StreamWriter(_fileStream)
            {
                AutoFlush = true
            };
        }

        public void Write(LogLevel logLevel, LogContext logContext, string msg)
        {
            if (logLevel >= _minLevel)
            {
                _writer.WriteLine(msg);
            }
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel >= _minLevel;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _writer?.Dispose();
                    _fileStream?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~StaticFileOutput()
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
