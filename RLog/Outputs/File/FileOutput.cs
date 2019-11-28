using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace RLog.Outputs.File
{
    public class FileOutput : ILogOutput, IDisposable
    {
        private readonly string _logPath;
        private readonly LogLevel _minLevel;

        private string _file;
        private StreamWriter _writer;

        public FileOutput(string logPath, LogLevel minLevel)
        {
            _logPath = logPath;
            _file = logPath;
            _minLevel = minLevel;
        }

        public void Write(LogLevel logLevel, LogContext logContext, string msg)
        {
            if (logLevel >= _minLevel)
            {
                WriteToFile(logContext, msg);
            }
        }

        private void WriteToFile(LogContext logContext, string msg)
        {
            string transformedPath = logContext.Format(_logPath);
            if (_file != transformedPath)
            {
                _writer?.Dispose();
                _writer = new StreamWriter(new FileStream(_file = transformedPath, FileMode.Append, FileAccess.Write, FileShare.Read));
            }

            _writer.WriteLine(msg);
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
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~FileOutput()
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
