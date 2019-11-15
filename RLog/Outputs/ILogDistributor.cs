using Microsoft.Extensions.Logging;

namespace RLog.Outputs
{
    public interface ILogDistributor
    {
        void Push(LogLevel logLevel, string msg);
    }
}
