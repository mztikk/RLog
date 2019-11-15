using Microsoft.Extensions.Logging;

namespace RLog.Outputs
{
    public interface ILogDistributor
    {
        bool IsEnabled(LogLevel logLevel);

        void Push(LogLevel logLevel, string msg);
    }
}
