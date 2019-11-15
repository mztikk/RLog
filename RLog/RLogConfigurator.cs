using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using RLog.Outputs;

namespace RLog
{
    public class RLogConfigurator
    {
        private LogLevel _logLevel = LogLevel.Information;
        private readonly ICollection<ILogOutput> _logOutputs = new List<ILogOutput>();
        private ILogDistributor _logDistributor = null;

        public RLogConfigurator SetLoglevel(LogLevel logLevel)
        {
            _logLevel = logLevel;
            return this;
        }

        public RLogConfigurator AddConsoleOutput(LogLevel logLevel)
        {
            _logOutputs.Add(new ConsoleOutput(logLevel));
            return this;
        }

        public RLogConfigurator AddConsoleOutput() => AddConsoleOutput(_logLevel);

        public ILogDistributor GetLogDistributor() => _logDistributor ?? new SerialDistributor(_logOutputs);
    }
}
