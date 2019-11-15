using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace RLog.Outputs
{
    public class SerialDistributor : ILogDistributor
    {
        private readonly IEnumerable<ILogOutput> _logOutputs;

        public SerialDistributor(IEnumerable<ILogOutput> logOutputs) => _logOutputs = logOutputs;

        public void Push(LogLevel logLevel, string msg)
        {
            foreach (ILogOutput logOutput in _logOutputs)
            {
                logOutput.Write(logLevel, msg);
            }
        }
    }
}
