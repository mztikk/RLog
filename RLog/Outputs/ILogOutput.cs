﻿using Microsoft.Extensions.Logging;

namespace RLog.Outputs
{
    public interface ILogOutput
    {
        bool IsEnabled(LogLevel logLevel);

        void Write(LogLevel logLevel, LogContext? logContext, string msg);
    }
}
