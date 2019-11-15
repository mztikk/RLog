using Microsoft.Extensions.Logging;

namespace RLog.Outputs
{
    public interface ILogOutput
    {
        void Write(LogLevel logLevel, string msg);
    }
}
