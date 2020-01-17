using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RLog.Outputs.Distribution
{
    public class QueueDistributor : ILogDistributor
    {
        private readonly IEnumerable<ILogOutput> _logOutputs;
        private readonly ConcurrentQueue<LogQueue> _logQueue;

        private Task _queueTask = Task.CompletedTask;

        public QueueDistributor(IEnumerable<ILogOutput> logOutputs)
        {
            _logQueue = new ConcurrentQueue<LogQueue>();

            _logOutputs = logOutputs;
        }

        public void Push(LogLevel logLevel, LogContext? logContext, string msg)
        {
            _logQueue.Enqueue(new LogQueue(logLevel, logContext, msg));

            if (_queueTask.IsCompleted)
            {
                _queueTask = Task.Factory.StartNew(WorkQueue);
            }
        }

        private void WorkQueue()
        {
            while (_logQueue.Count > 0)
            {
                if (_logQueue.TryDequeue(out LogQueue item))
                {
                    foreach (ILogOutput output in _logOutputs)
                    {
                        output.Write(item.LogLevel, item.LogContext, item.LogMsg);
                    }
                }
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
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
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

                _disposedValue = true;
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

        private class LogQueue
        {
            public LogLevel LogLevel { get; set; }
            public LogContext? LogContext { get; set; }
            public string LogMsg { get; set; }

            public LogQueue() { }

            public LogQueue(LogLevel logLevel, LogContext? logContext, string logMsg)
            {
                LogLevel = logLevel;
                LogContext = logContext;
                LogMsg = logMsg;
            }
        }
    }
}
