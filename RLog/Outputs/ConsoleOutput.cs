using System;
using Microsoft.Extensions.Logging;

namespace RLog.Outputs
{
    public class ConsoleOutput : ILogOutput
    {
        private readonly LogLevel _minLevel;

        public ConsoleOutput(LogLevel minLevel) => _minLevel = minLevel;

        public void Write(LogLevel logLevel, string msg)
        {
            if (logLevel >= _minLevel)
            {
                Console.WriteLine(msg);
            }
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel >= _minLevel;
    }
}
