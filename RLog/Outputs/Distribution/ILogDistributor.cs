using System;
using Microsoft.Extensions.Logging;

namespace RLog.Outputs.Distribution
{
    public interface ILogDistributor : IDisposable
    {
        bool IsEnabled(LogLevel logLevel);

        void Push(LogLevel logLevel, LogContext logContext, string msg);
    }
}
