using Microsoft.Extensions.Logging;

namespace RLog.Outputs.Console
{
    public class ConsoleOutput : ILogOutput
    {
        private readonly LogLevel _minLevel;

        public ConsoleOutput(LogLevel minLevel) => _minLevel = minLevel;

        public void Write(LogLevel logLevel, LogContext logContext, string msg)
        {
            if (logLevel >= _minLevel)
            {
                System.Console.WriteLine(msg);
            }
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel >= _minLevel;
    }
}
